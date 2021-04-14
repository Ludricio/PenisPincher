using System;
using System.Collections.Generic;
using Discord;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;

namespace PenisPincher.Discord
{
    public class DiscordServerLogBuilder : IDiscordServerLogBuilder, IDiscordServerLogLocationBuilder,
        IDiscordServerLogMessageBuilder, IDiscordServerLogReasonBuilder, IDiscordServerLogEmbedBuilder
    {
        public DiscordServerLogBuilder(IOptions<AppConfig.DiscordConfig> configuration)
        {
            Color = configuration.Value.EmbedColor;
            LogTitle = null;
            Location = null;
            Reason = null;
            Message = null;
        }

        private Color Color { get; }
        private string LogTitle { get; set; }
        private Tuple<string, object[]> Location { get; set; }
        private Tuple<string, object[]> Message { get; set; }
        private Tuple<string, object[]> Reason { get; set; }

        public Embed Build()
        {
            var builder = new EmbedBuilder()
                .WithTitle(LogTitle)
                .WithColor(Color)
                .WithFields(ConvertToEmbedFieldBuilders())
                .WithCurrentTimestamp();
            return builder.Build();
        }

        public IDiscordServerLogLocationBuilder WithTitle(string errorTitle)
        {
            LogTitle = errorTitle;
            return this;
        }

        IDiscordServerLogMessageBuilder IDiscordServerLogLocationBuilder.WithLocation(string location,
            params object[] parameters)
        {
            Location = Tuple.Create(location, parameters);
            return this;
        }

        IDiscordServerLogReasonBuilder IDiscordServerLogMessageBuilder.WithMessage(string message,
            params object[] parameters)
        {
            Message = Tuple.Create(message, parameters);
            return this;
        }

        IDiscordServerLogEmbedBuilder IDiscordServerLogReasonBuilder.WithReason(string reason,
            params object[] parameters)
        {
            Reason = Tuple.Create(reason, parameters);
            return this;
        }

        private IEnumerable<EmbedFieldBuilder> ConvertToEmbedFieldBuilders()
        {
            return new EmbedFieldBuilder[]
            {
                new()
                {
                    Name = nameof(Location),
                    Value = string.Format(Location.Item1, Location.Item2)
                },
                new()
                {
                    Name = nameof(Reason),
                    Value = string.Format(Reason.Item1, Reason.Item2)
                },
                new()
                {
                    Name = nameof(Message),
                    Value = string.Format(Message.Item1, Message.Item2)
                }
            };
        }
    }

    public interface IDiscordServerLogBuilder
    {
        IDiscordServerLogLocationBuilder WithTitle(string errorTitle);
        Embed Build();
    }

    public interface IDiscordServerLogLocationBuilder
    {
        [StringFormatMethod("location")]
        IDiscordServerLogMessageBuilder WithLocation(string location, params object[] parameters);
    }

    public interface IDiscordServerLogMessageBuilder
    {
        [StringFormatMethod("message")]
        IDiscordServerLogReasonBuilder WithMessage(string message, params object[] parameters);
    }

    public interface IDiscordServerLogReasonBuilder
    {
        [StringFormatMethod("reason")]
        IDiscordServerLogEmbedBuilder WithReason(string reason, params object[] parameters);
    }

    public interface IDiscordServerLogEmbedBuilder
    {
        Embed Build();
    }
}