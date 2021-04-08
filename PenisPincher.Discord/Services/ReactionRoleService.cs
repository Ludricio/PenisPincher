using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using JetBrains.Annotations;
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
        private IDiscordServerLogBuilder ServerLogBuilder { get; }

        private Dictionary<ulong, Dictionary<string, ReactionRole>> MonitoredReactions { get; }

        public ReactionRoleService([NotNull] DiscordSocketClient discordClient,[NotNull] ILogger<ReactionRoleService> logger, [NotNull] IDiscordServerLogBuilder serverLogBuilder) : this(
            discordClient, logger, serverLogBuilder, Enumerable.Empty<ReactionRole>())
        {
            
        }

        public ReactionRoleService([NotNull] DiscordSocketClient discordClient,
            [NotNull] ILogger<ReactionRoleService> logger, [NotNull] IDiscordServerLogBuilder serverLogBuilder,
            [NotNull] IEnumerable<ReactionRole> reactionRoles)
        {
            DiscordClient = discordClient ?? throw new ArgumentNullException(nameof(discordClient));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            ServerLogBuilder = serverLogBuilder;
            if(reactionRoles is null) throw new ArgumentNullException(nameof(reactionRoles));
            DiscordClient.ReactionAdded += OnReactionAdded;
            DiscordClient.ReactionRemoved += OnReactionRemoved;

            MonitoredReactions =
                reactionRoles
                    .GroupBy(x => x.MessageId)
                    .ToDictionary(group => group.Key, group =>
                        group.ToDictionary(
                            x => x.EmoteName,
                            x => x));
        }

        private async Task OnReactionAdded(Cacheable<IUserMessage, ulong> cache, ISocketMessageChannel smc, SocketReaction sr)
        {
            if(sr.UserId == DiscordClient.CurrentUser.Id) return;
            if (smc is not SocketTextChannel textChannel) return;
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
                ServerLogBuilder
                    .WithTitle("Error")
                    .WithLocation(nameof(ReactionRoleService))
                    .WithMessage("Could not assign role for reaction on emote {0} on message {1} by user id {2}",
                        reactionRole.EmoteName, reactionRole.MessageId, sr.UserId)
                    .WithReason("No role with ID {0} exists", reactionRole.RoleId);
                await textChannel.Guild.LogErrorToServerAsync(reactionRole.OwningServer, ServerLogBuilder);
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
                ServerLogBuilder
                    .WithTitle("Error")
                    .WithLocation(nameof(ReactionRoleService))
                    .WithMessage("Could not revoke role for reaction on emote {0} on message {1} by user id {2}",
                        reactionRole.EmoteName, reactionRole.MessageId, sr.UserId)
                    .WithReason("No role with ID {0} exists", reactionRole.RoleId);
                await textChannel.Guild.LogErrorToServerAsync(reactionRole.OwningServer, ServerLogBuilder);
            }
        }

        public void AddMonitoredReaction(ReactionRole reactionRole)
        {
            if(!MonitoredReactions.TryGetValue(reactionRole.Id, out var reactionSet))
            {
                reactionSet = new Dictionary<string, ReactionRole>();
                MonitoredReactions.Add(reactionRole.Id, reactionSet);
                //TODO add bot's reaction to message
            }

            reactionSet[reactionRole.EmoteName] = reactionRole;
            Logger.Information("Added role reaction monitoring for emote {0} on message {1} for role {2}",
                reactionRole.EmoteName, reactionRole.MessageId, reactionRole.RoleId);
        }

        public void RemoveMonitoredReaction(ReactionRole reactionRole)
        {
            if(!MonitoredReactions.TryGetValue(reactionRole.Id, out var reactionSet)) return;

            reactionSet.Remove(reactionRole.EmoteName, out _);
            Logger.Information("Removed role reaction monitoring for emote {0} on message {1} for role {2}",
                reactionRole.EmoteName, reactionRole.MessageId, reactionRole.RoleId);
            //TODO remove bot's reaction from message
        }
    }
}
