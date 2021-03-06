using System;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PenisPincher.Core.Models;
using PenisPincher.Discord.Services;
using PenisPincher.Persistence;
using PenisPincher.Utilities;
using PenisPincher.Utilities.Extensions;

namespace PenisPincher.Discord
{
    public class DiscordLauncher : AsyncDisposable
    {
        public DiscordLauncher(ILogger<DiscordLauncher> logger)
        {
            Logger = logger;
            WaitHandle = new ManualResetEvent(false);
        }

        private ILogger<DiscordLauncher> Logger { get; }
        private ManualResetEvent WaitHandle { get; }

        public async Task StartDiscordServicesAsync(IServiceProvider serviceProvider)
        {
            Logger.Information("Starting Discord services");
            Logger.Information("Starting service: {0}", nameof(DiscordLoggingService));
            serviceProvider.GetRequiredService<DiscordLoggingService>();

            Logger.Information("Starting service: {0}", nameof(CommandHandlerService));
            serviceProvider.GetRequiredService<CommandHandlerService>();

            Logger.Information("Starting service: {0}", nameof(DiscordLoginService));
            var loginService = serviceProvider.GetRequiredService<DiscordLoginService>();

            Logger.Information("Starting Service: {0}", nameof(ReactionRoleService));
            serviceProvider.GetRequiredService<ReactionRoleService>();

            Logger.Information("Starting Service: {0}", nameof(StreamLiveMonitorService));
            serviceProvider.GetRequiredService<StreamLiveMonitorService>();


            loginService.OnClientReady += () =>
            {
                WaitHandle.Set();
                return Task.CompletedTask;
            };

            await loginService.StartAsync();
            WaitHandle.WaitOne();
        }

        protected override void Dispose(bool disposing)
        {
            if (Disposed) return;
            if (disposing) WaitHandle.Dispose();
        }
    }

    public static class DiscordServiceConfigurationExtensions
    {
        public static IServiceCollection AddDiscordServices(this IServiceCollection services)
        {
            services
                .AddSingleton(new DiscordSocketClient(new DiscordSocketConfig
                {
#if DEBUG
                    LogLevel = LogSeverity.Info,
#else
                LogLevel = LogSeverity.Verbose,
#endif
                    MessageCacheSize = 1000
                }))
                .AddSingleton(new CommandService(new CommandServiceConfig
                {
#if DEBUG
                    LogLevel = LogSeverity.Info,
#else
                    LogLevel = LogSeverity.Verbose,
#endif
                    DefaultRunMode = RunMode.Async
                }))
                .AddSingleton<DiscordLoggingService>()
                .AddSingleton<CommandHandlerService>()
                .AddSingleton<DiscordLoginService>()
                .AddSingleton<StreamLiveMonitorService>()
                .AddTransient<IDiscordServerLogBuilder, DiscordServerLogBuilder>()
                .AddSingleton(provider =>
                {
                    var discordClient = provider.GetRequiredService<DiscordSocketClient>();
                    var logger = provider.GetRequiredService<ILogger<ReactionRoleService>>();
                    var serverLogBuilder = provider.GetRequiredService<IDiscordServerLogBuilder>();
                    using var scope = provider.CreateScope();
                    var repo = scope.ServiceProvider.GetRequiredService<IRepository<ReactionRole>>();
                    return new ReactionRoleService(discordClient, logger, serverLogBuilder, repo.Get());
                });
            return services;
        }
    }
}