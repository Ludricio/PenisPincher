using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using PenisPincher.Persistence;
using PenisPincher.Utilities.Extensions;

namespace PenisPincher.Discord
{
    public class Bot
    {
        private Bot()
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("_config.json")
                .AddJsonFile("dbconfig.json")
                .Build();
        }

        private IConfigurationRoot Configuration { get; }
        private ILogger<Bot> Logger { get; set; }

        public static async Task RunAsync()
        {
            await new Bot().Run();
        }

        private async Task Run()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);

            await using var provider = services.BuildServiceProvider();

            Logger = provider.GetRequiredService<ILogger<Bot>>();
            Logger.Information("All services configured, begin initialization");

            await provider.GetRequiredService<DbContext>().Database.MigrateAsync();

            try
            {
                await using var launcher = provider.GetRequiredService<DiscordLauncher>();

                Logger.Information("Entering async wait loop, event reaction enabled.");
                await Task.Delay(-1);
            }
            catch (Exception e)
            {
                Logger.Critical(e, "Uncaught exception");
                throw;
            }
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services
                .AddDbContext<DbContext, PenisPincherDbContext>(options =>
                    options.UseSqlServer(Configuration["Database:ConnectionString"]))
                .AddLogging(builder =>
                {
                    builder.ClearProviders();
#if DEBUG
                    builder.SetMinimumLevel(LogLevel.Trace);
#else
                builder.SetMinimumLevel(LogLevel.Information);
#endif
                    builder.AddNLog(Configuration);
                })
                .AddDiscordServices()
                .AddSingleton<Random>()
                .AddSingleton(Configuration)
                .AddSingleton<DiscordLauncher>()

                //Configure IOptions
                .Configure<AppConfig.DatabaseConfig>(options =>
                    Configuration.GetSection("Database").Bind(options, c => c.BindNonPublicProperties = true))
                .Configure<AppConfig.DiscordConfig>(options =>
                    Configuration.GetSection("Discord").Bind(options, c => c.BindNonPublicProperties = true));
        }
    }
}