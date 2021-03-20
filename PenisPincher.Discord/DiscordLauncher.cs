using System;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PenisPincher.Discord.Services;
using PenisPincher.Utilities;

namespace PenisPincher.Discord
{

    public class DiscordLauncher : AsyncDisposable
    {
        private ILogger<DiscordLauncher> Logger { get; set; }
        private ManualResetEvent WaitHandle { get; }
        public DiscordLauncher(ILogger<DiscordLauncher> logger)
        {
            Logger = logger;
            WaitHandle = new ManualResetEvent(false);
        }

        public async Task StartDiscordServicesAsync(IServiceProvider serviceProvider)
        {

        }

        public static void ConfigureServices(IServiceCollection services)
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
                .AddSingleton<DiscordLoginService>();
        }

        protected override void Dispose(bool disposing)
        {
            if(Disposed) return;
            if(disposing)
            {
                WaitHandle.Dispose();
            }
        }
    }
}