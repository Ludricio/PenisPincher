using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PenisPincher.Utilities.Extensions;

namespace PenisPincher.Discord.Services
{
    public class DiscordLoginService
    {
        public DiscordLoginService(IServiceProvider serviceProvider, DiscordSocketClient discordClient,
            CommandService commandService, IOptions<AppConfig.DiscordConfig> configuration,
            ILogger<DiscordLoginService> logger)
        {
            ServiceProvider = serviceProvider;
            DiscordClient = discordClient;
            CommandService = commandService;
            Configuration = configuration.Value;
            Logger = logger;
        }

        private IServiceProvider ServiceProvider { get; }
        private DiscordSocketClient DiscordClient { get; }
        private CommandService CommandService { get; }
        private AppConfig.DiscordConfig Configuration { get; }
        private ILogger<DiscordLoginService> Logger { get; }

        public event Func<Task> OnClientReady
        {
            add => DiscordClient.Ready += value;
            remove => DiscordClient.Ready -= value;
        }

        public async Task StartAsync()
        {
            Logger.Debug("Configuring discord session");
            var token = Configuration.Token;
            if (token.IsNullOrWhitespace())
            {
                Logger.Error("No discord bot token in _config.yaml");
                throw new Exception("Enter bot token into the `_config.yaml` file");
            }

            Logger.Debug("Setting up discord session");
            await DiscordClient.LoginAsync(TokenType.Bot, token);
            Logger.Debug("Starting discord client");
            await DiscordClient.StartAsync();

            Logger.Debug("Loading modules");
            await CommandService.AddModulesAsync(Assembly.GetEntryAssembly(), ServiceProvider);
        }
    }
}