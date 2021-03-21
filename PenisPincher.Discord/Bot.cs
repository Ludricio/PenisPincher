using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using PenisPincher.Persistence;
using PenisPincher.Utilities.Extensions;

namespace PenisPincher.Discord
{
    public class Bot
    {
        private IConfigurationRoot Configuration { get; }
        private ILogger<Bot> Logger { get; set; }

        private Bot()
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddYamlFile("_config.yaml")
                .Build();
        }

        public static async Task RunAsync() => await new Bot().Run();

        private async Task Run()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            

            await using var provider = services.BuildServiceProvider();
            Logger = provider.GetRequiredService<ILogger<Bot>>();
            Logger.Information("All services configured, begin initialization");

            try
            {
                EnsureDatabaseValidity(provider);

                await using var launcher = provider.GetRequiredService<DiscordLauncher>();

                Logger.Information("Entering async wait loop, event reaction enabled.");
                await Task.Delay(-1);
            }
            catch(Exception e)
            {
                Logger.Critical(e, "Uncaught exception");
                throw;
            }
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(builder =>
                {
                    builder.ClearProviders();
#if DEBUG
                    builder.SetMinimumLevel(LogLevel.Trace);
#else
                builder.SetMinimumLevel(LogLevel.Information);
#endif
                    builder.AddNLog(Configuration);
                })
                .AddPersistence(Configuration.ConnectionString())
                .AddDiscordServices()
                .AddSingleton<Random>()
                .AddSingleton(Configuration)
                .AddSingleton<DiscordLauncher>();
        }

        private async void EnsureDatabaseValidity(IServiceProvider provider)
        {
            await using var context = provider.GetRequiredService<DbContext>();
            Logger.Information("Ensuring database validity...");
            await context.Database.EnsureCreatedAsync();
            Logger.Information("Database valid");
        }
    }
}
