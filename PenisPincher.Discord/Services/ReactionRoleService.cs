using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;

namespace PenisPincher.Discord.Services
{
    public class ReactionRoleService
    {
        private DiscordSocketClient DiscordClient { get; }

        private ILogger<ReactionRoleService> Logger { get; }

        public ReactionRoleService(DiscordSocketClient discordClient, ILogger<ReactionRoleService> logger)
        {
            DiscordClient = discordClient;
            Logger = logger;

            DiscordClient.ReactionAdded += OnReactionAdded;
            DiscordClient.ReactionRemoved += OnReactionRemoved;
        }

        //TODO add Role reaction model with message id and emote properties for configuration and identification
        private async Task OnReactionAdded(Cacheable<IUserMessage, ulong> cache, ISocketMessageChannel smc, SocketReaction sr)
        {
            throw new NotImplementedException();
        }

        private async Task OnReactionRemoved(Cacheable<IUserMessage, ulong> cache, ISocketMessageChannel smc, SocketReaction sr)
        {
            throw new NotImplementedException();
        }
    }
}
