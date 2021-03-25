using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using PenisPincher.Core.Models;

namespace PenisPincher.Discord.Services
{
    public class ReactionRoleService
    {
        private DiscordSocketClient DiscordClient { get; }

        private ILogger<ReactionRoleService> Logger { get; }

        private ConcurrentDictionary<ulong, ConcurrentDictionary<string, ReactionRole>> MonitoredReactions { get; }

        public ReactionRoleService(DiscordSocketClient discordClient, ILogger<ReactionRoleService> logger)
        {
            DiscordClient = discordClient;
            Logger = logger;
            MonitoredReactions = new ConcurrentDictionary<ulong, ConcurrentDictionary<string, ReactionRole>>();

            DiscordClient.ReactionAdded += OnReactionAdded;
            DiscordClient.ReactionRemoved += OnReactionRemoved;
        }

        //TODO add Role reaction model with message id and emote properties for configuration and identification
        private async Task OnReactionAdded(Cacheable<IUserMessage, ulong> cache, ISocketMessageChannel smc, SocketReaction sr)
        {
            if(sr.UserId == DiscordClient.CurrentUser.Id) return;

            if(smc is not SocketTextChannel textChannel) return;
            if(!MonitoredReactions.TryGetValue(sr.MessageId, out var reactionSet)) return;
            if(!reactionSet.TryGetValue(sr.Emote.Name, out var reactionRole)) return;

            //TODO get Role from guild and add to GuildUser (get cached from sr or async from guild).
            //TODO log as information
        }

        private async Task OnReactionRemoved(Cacheable<IUserMessage, ulong> cache, ISocketMessageChannel smc, SocketReaction sr)
        {
            if (sr.UserId == DiscordClient.CurrentUser.Id) return;

            if (smc is not SocketTextChannel textChannel) return;
            if (!MonitoredReactions.TryGetValue(sr.MessageId, out var reactionSet)) return;
            if (!reactionSet.TryGetValue(sr.Emote.Name, out var reactionRole)) return;

            //TODO get Role from guild and strip from GuildUser (get cached from sr or async from guild).
            //TODO log as information
        }

        public void AddMonitoredReaction(ReactionRole reactionRole)
        {
            if(!MonitoredReactions.TryGetValue(reactionRole.Id, out var reactionSet))
            {
                reactionSet = new ConcurrentDictionary<string, ReactionRole>();
                MonitoredReactions.TryAdd(reactionRole.Id, reactionSet);
                //TODO add bot's reaction to message
            }

            reactionSet.GetOrAdd(reactionRole.EmoteName, reactionRole);
        }

        public void RemoveMonitoredReaction(ReactionRole reactionRole)
        {
            if(!MonitoredReactions.TryGetValue(reactionRole.Id, out var reactionSet)) return;

            reactionSet.TryRemove(reactionRole.EmoteName, out _);
            //TODO remove bot's reaction from message
        }
    }
}
