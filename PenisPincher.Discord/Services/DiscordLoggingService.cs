using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;

namespace PenisPincher.Discord.Services
{
    public class DiscordLoggingService
    {
        private DiscordSocketClient DiscordClient { get; }
        private CommandService CommandService { get; }
        private ILogger<DiscordLoggingService> Logger { get; }

        public DiscordLoggingService(DiscordSocketClient discordClient, CommandService commandService,
                                     ILogger<DiscordLoggingService> logger)
        {
            DiscordClient = discordClient;
            CommandService = commandService;
            Logger = logger;

            DiscordClient.Log += OnLogAsync;
            CommandService.Log += OnLogAsync;
        }

        private Task OnLogAsync(LogMessage msg)
        {
            var level = msg.Severity switch
            {
                LogSeverity.Critical => LogLevel.Critical,
                LogSeverity.Error => LogLevel.Error,
                LogSeverity.Warning => LogLevel.Warning,
                LogSeverity.Info => LogLevel.Information,
                LogSeverity.Verbose => LogLevel.Trace,
                LogSeverity.Debug => LogLevel.Debug,
                _ => throw new ArgumentOutOfRangeException()
            };
            Logger.Log(level, msg.Exception, "{0}: {1}", msg.Source, msg.Message);
            return Task.CompletedTask;
        }
    }
}
