using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using PenisPincher.Core.Models;

namespace PenisPincher.Discord.Extensions
{
    public static class LoggingExtensions
    {
        public static async Task LogToServerAsync(this SocketGuild @this, DiscordServerLogLevel logLevel, DiscordServerConfiguration serverConfig, IDiscordServerLogBuilder embedBuilder)
        {
            if(!serverConfig.LogToServer || logLevel < serverConfig.LogLevel) return;

            var channel = @this.GetTextChannel(serverConfig.ServerLogChannelId);
            if (channel is null) return;
            
            await channel.SendMessageAsync("", false, embedBuilder.Build());
        }

        public static async Task LogDebugToServerAsync(this SocketGuild @this,
                                                             DiscordServerConfiguration serverConfig,
                                                             IDiscordServerLogBuilder embedBuilder)
        {
            await LogToServerAsync(@this, DiscordServerLogLevel.Verbose, serverConfig, embedBuilder);
        }

        public static async Task LogInformationToServerAsync(this SocketGuild @this,
                                                             DiscordServerConfiguration serverConfig,
                                                            IDiscordServerLogBuilder embedBuilder)
        {
            await LogToServerAsync(@this, DiscordServerLogLevel.Information, serverConfig, embedBuilder);
        }

        public static async Task LogErrorToServerAsync(this SocketGuild @this,
                                                       DiscordServerConfiguration serverConfig,
                                                       IDiscordServerLogBuilder embedBuilder)
        {
            await LogToServerAsync(@this, DiscordServerLogLevel.Error, serverConfig, embedBuilder);
        }
    }
}
