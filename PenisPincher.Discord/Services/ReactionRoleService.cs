using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using PenisPincher.Core.Models;
using PenisPincher.Discord.Extensions;
using PenisPincher.Utilities.Extensions;

namespace PenisPincher.Discord.Services
{
    public class ReactionRoleService
    {
        private DiscordSocketClient DiscordClient { get; }

        private ILogger<ReactionRoleService> Logger { get; }

        private ConcurrentDictionary<ulong, ConcurrentDictionary<string, ReactionRole>> MonitoredReactions { get; }

        public ReactionRoleService(DiscordSocketClient discordClient, ILogger<ReactionRoleService> logger) : this(
            discordClient, logger, Enumerable.Empty<ReactionRole>())
        {
        }

        public ReactionRoleService(DiscordSocketClient discordClient, ILogger<ReactionRoleService> logger, IEnumerable<ReactionRole> reactionRoles)
        {
            DiscordClient = discordClient;
            Logger = logger;
            MonitoredReactions = new ConcurrentDictionary<ulong, ConcurrentDictionary<string, ReactionRole>>();
            DiscordClient.ReactionAdded += OnReactionAdded;
            DiscordClient.ReactionRemoved += OnReactionRemoved;

            var dict = new ConcurrentDictionary<ulong, ConcurrentDictionary<string, ReactionRole>>(
                reactionRoles.GroupBy(x => x.MessageId)
                .ToDictionary(group => group.Key,
                    group => new ConcurrentDictionary<string, ReactionRole>(group.ToDictionary(
                        x => x.EmoteName,
                        x => x))));


        }

        private async Task OnReactionAdded(Cacheable<IUserMessage, ulong> cache, ISocketMessageChannel smc, SocketReaction sr)
        {
            if(sr.UserId == DiscordClient.CurrentUser.Id) return;

            if(smc is not SocketTextChannel textChannel) return;
            if(!MonitoredReactions.TryGetValue(sr.MessageId, out var reactionSet)) return;
            if(!reactionSet.TryGetValue(sr.Emote.Name, out var reactionRole)) return;

            var role = textChannel.Guild.GetRole(reactionRole.Id);
            if (role is not null)
            {
                var user = sr.User.IsSpecified ? (SocketGuildUser)sr.User.Value : textChannel.GetUser(sr.UserId);
                await user.AddRoleAsync(role);
            }
            else
            {
                //if role doesn't exist in discord, log and exit.
                Logger.Warning(
                    "Could not assign role {0}, role does not exist. GuildID: {1} MessageID: {2} EmoteName: {3}",
                    reactionRole.RoleId, textChannel.Guild.Id, reactionRole.MessageId, reactionRole.EmoteName);
                await textChannel.Guild.LogErrorToServerAsync(reactionRole.OwningServer, //TODO change to embedbuilder and remove text variant from extension
                    string.Format(
                        @"**ERROR**
                    Location: ReactionRoleService
                    Message: Could not assign role for reaction on emote {0} on message {1} by user id {2}.
                    Reason: No role with ID {3} exists", reactionRole.EmoteName, reactionRole.MessageId, sr.UserId, reactionRole.RoleId));
            }
        }

        private async Task OnReactionRemoved(Cacheable<IUserMessage, ulong> cache, ISocketMessageChannel smc, SocketReaction sr)
        {
            if (sr.UserId == DiscordClient.CurrentUser.Id) return;

            if (smc is not SocketTextChannel textChannel) return;
            if (!MonitoredReactions.TryGetValue(sr.MessageId, out var reactionSet)) return;
            if (!reactionSet.TryGetValue(sr.Emote.Name, out var reactionRole)) return;

            var role = textChannel.Guild.GetRole(reactionRole.Id);
            if(role is not null)
            {
                var user = sr.User.IsSpecified ? (SocketGuildUser)sr.User.Value : textChannel.GetUser(sr.UserId);
                await user.RemoveRoleAsync(role);
            }
            else
            {
                //if role doesn't exist in discord, log and exit.
                Logger.Warning(
                    "Could not assign role {0}, role does not exist. GuildID: {1} MessageID: {2} EmoteName: {3}",
                    reactionRole.RoleId, textChannel.Guild.Id, reactionRole.MessageId, reactionRole.EmoteName);
                await textChannel.Guild.LogErrorToServerAsync(reactionRole.OwningServer, //TODO change to embedbuilder and remove text variant from extension
                    string.Format(
                        @"**ERROR**
                    Location: ReactionRoleService
                    Message: Could not revoke role for reaction on emote {0} on message {1} by user id {2}.
                    Reason: No role with ID {3} exists", reactionRole.EmoteName, reactionRole.MessageId, sr.UserId, reactionRole.RoleId));
            }
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
            Logger.Information("Added role reaction monitoring for emote {0} on message {1} for role {2}",
                reactionRole.EmoteName, reactionRole.MessageId, reactionRole.RoleId);
        }

        public void RemoveMonitoredReaction(ReactionRole reactionRole)
        {
            if(!MonitoredReactions.TryGetValue(reactionRole.Id, out var reactionSet)) return;

            reactionSet.TryRemove(reactionRole.EmoteName, out _);
            Logger.Information("Removed role reaction monitoring for emote {0} on message {1} for role {2}",
                reactionRole.EmoteName, reactionRole.MessageId, reactionRole.RoleId);
            //TODO remove bot's reaction from message
        }
    }
}
