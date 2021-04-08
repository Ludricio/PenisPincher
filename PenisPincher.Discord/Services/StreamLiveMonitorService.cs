using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using JetBrains.Annotations;
using PenisPincher.Core.Models;
using PenisPincher.Discord.Extensions;
using PenisPincher.Utilities.Extensions;
using TwitchLib.Api.Interfaces;
using TwitchLib.Api.Services;
using TwitchLib.Api.Services.Events.LiveStreamMonitor;

namespace PenisPincher.Discord.Services
{
    public class StreamLiveMonitorService
    {
        private readonly object _lock;
        private DiscordSocketClient DiscordClient { get; }
        private ITwitchAPI TwitchApi { get; }
        private EmbedBuilder EmbedBuilder { get; }
        private Dictionary<string, List<MonitoredStream>> MonitoredStreams { get; }
        private LiveStreamMonitorService LiveStreamMonitor { get; }
        public bool IsRunning => LiveStreamMonitor.Enabled;

        private const string ChannelReplaceTag = "{channel}";
        private readonly Regex _notificationRoleRegex = new("{notif<[0-9]*>}", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public StreamLiveMonitorService(DiscordSocketClient discordClient, ITwitchAPI twitchApi, EmbedBuilder embedBuilder, IEnumerable<MonitoredStream> streams)
        {
            _lock = new object();
            DiscordClient = discordClient;
            TwitchApi = twitchApi;
            EmbedBuilder = embedBuilder;

            LiveStreamMonitor = new LiveStreamMonitorService(TwitchApi);
            LiveStreamMonitor.OnStreamOnline += OnStreamOnline;

            MonitoredStreams = streams
                .GroupBy(x => x.StreamName.ToLower())
                .ToDictionary(
                    x => x.Key, 
                    x => x.ToList());

            if (MonitoredStreams.Values.SelectMany(x => x).ToList().Count > 0)
            {
                LiveStreamMonitor.Start();
            }
        }

        public void AddStream([NotNull] MonitoredStream stream)
        {
            stream.ThrowIfNull(nameof(stream));
            lock (_lock)
            {
                //if stream is already monitored just add to the backwards reference list
                if (MonitoredStreams.TryGetValue(stream.StreamName.ToLower(), out var streams))
                { 
                    streams.Add(stream);
                    return;
                }

                //Add new stream and set monitoring
                MonitoredStreams.Add(stream.StreamName.ToLower(), new List<MonitoredStream> { stream });
                LiveStreamMonitor.SetChannelsByName(MonitoredStreams.Keys.ToList());

                //If key count is 1 start monitoring service
                if (MonitoredStreams.Keys.Count == 1)
                {
                    LiveStreamMonitor.Start();
                }
            }
        }

        public void RemoveStream([NotNull] MonitoredStream stream)
        {
            stream.ThrowIfNull(nameof(stream));

            lock (_lock)
            {
                //If stream is not monitored nothing needs to be done
                if (!MonitoredStreams.TryGetValue(stream.StreamName.ToLower(), out var streams)) return;

                var removed = streams.Remove(stream);
                //If nothing was removed or backwards ref list is not empty nothing needs to be done
                if (!removed || streams.Count is not 0) return;

                MonitoredStreams.Remove(stream.StreamName.ToLower());

                //if key count is 0 stop monitoring service
                if (MonitoredStreams.Keys.Count == 0)
                {
                    LiveStreamMonitor.Stop();
                }
            }
        }

        private async void OnStreamOnline([CanBeNull]object sender, OnStreamOnlineArgs e)
        {
            IImmutableList<MonitoredStream> streams;
            lock (_lock)
            {
                streams = MonitoredStreams[e.Channel.ToLower()].ToImmutableList();
            }

            var thumbnailUrl = e.Stream.ThumbnailUrl.Replace("{width}", "320").Replace("{height}", "180");

            //Get user profile picture
            var profileImgUrl = (await TwitchApi.Helix.Users.GetUsersAsync(new List<string> {e.Stream.GameId}))
                .Users.FirstOrDefault()?.ProfileImageUrl;

            //Prepare the embedded rich text
            var embed = EmbedBuilder
                .WithAuthor(x =>
                {
                    x.WithIconUrl(profileImgUrl);
                    x.WithName(e.Channel);
                })
                .WithDescription($"[{e.Stream.Title}](https://www.twitch.tv/{e.Channel})")
                .WithThumbnailUrl(profileImgUrl)
                .AddField("Game", e.Stream.GameName, true)
                .AddField("Viewers", e.Stream.ViewerCount, true)
                .WithImageUrl(thumbnailUrl)
                .WithFooter($"Went live at: {e.Stream.StartedAt.ToString(CultureInfo.InvariantCulture)}")
                .Build();

            await NotifyChannelsAsync(streams, embed);
        }

        private async Task NotifyChannelsAsync(IImmutableList<MonitoredStream> streams, Embed embed)
        {
            foreach (var stream in streams)
            {
                var guild = DiscordClient.GetGuild(stream.OwningServer.ServerId);
                var notificationChannel = guild.GetTextChannel(stream.OwningServer.StreamNotificationChannelId);
                if (notificationChannel is null)
                {
                    await guild.LogToServerAsync(DiscordServerLogLevel.Error, stream.OwningServer,
                        null); //TODO fix logging,
                    continue;
                }

                //Format message according to template
                var notificationMessage = FormatNotificationMessage(
                    stream.NotificationTemplate,
                    stream.StreamName,
                    stream.NotificationRoleIds.Select(x => guild.GetRole(x)).ToImmutableList());

                //Send notification to server
                await notificationChannel.SendMessageAsync(notificationMessage, false, embed);
            }
        }

        private string FormatNotificationMessage(string template, string channelName, IImmutableList<SocketRole> roles)
        {
            //Replace all channel tags with channel name
            string notificationMessage = template.Replace(ChannelReplaceTag, channelName, StringComparison.OrdinalIgnoreCase);
            try
            {
                var index = 0;
                //Replace each notification tag with notification roles in the order they appear in the list
                _notificationRoleRegex.Replace(template, m => roles[index++].Mention);
            }
            catch (IndexOutOfRangeException ex)
            {
                //TODO add logging to local and server
            }

            return notificationMessage;

        }
    }
}
