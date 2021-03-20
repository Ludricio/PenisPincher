using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;

namespace PenisPincher.Discord.Services
{
    public class CommandHandlerService
    {
        private DiscordSocketClient DiscordClient { get; }
        private CommandService CommandService { get; }
        private IConfiguration Configuration { get; }
        private IServiceProvider ServiceProvider { get; }
        private Color EmbedColor { get; }

        public CommandHandlerService(DiscordSocketClient discordClient, CommandService commandService,
                                     IConfiguration configuration, IServiceProvider serviceProvider)
        {
            DiscordClient = discordClient;
            CommandService = commandService;
            Configuration = configuration;
            ServiceProvider = serviceProvider;
            EmbedColor = new Color(configuration.DiscordEmbedColor());
            DiscordClient.MessageReceived += OnMessageReceived;
        }

        private async Task OnMessageReceived(SocketMessage sm)
        {
            if(sm is not SocketUserMessage msg) return;
            if(msg.Author.Id == DiscordClient.CurrentUser.Id) return;

            var context = new SocketCommandContext(DiscordClient, msg);

            var argPos = 0;
            if(!msg.HasStringPrefix("!", ref argPos) &&
               !msg.HasMentionPrefix(DiscordClient.CurrentUser, ref argPos)) return;

            var result = await CommandService.ExecuteAsync(context, argPos, ServiceProvider);

            if(result.IsSuccess) return;

            var retMsg = $"Failed executing command **{msg}**.\nReason: {result.ErrorReason}";
            if(result.Error != null)
                retMsg += result.Error.Value switch
                {
                    CommandError.UnknownCommand => "Unknown command",
                    CommandError.BadArgCount => "Wrong argument count for command",
                    _ => "Uknown error"
                };

            var builder = new EmbedBuilder()
                .WithColor(EmbedColor)
                .WithDescription($"Failed executing command **{msg}**")
                .AddField($"Reason: {result.ErrorReason}", retMsg);

            await msg.Channel.SendMessageAsync("", embed: builder.Build());
            await msg.DeleteAsync();
        }
    }
}
