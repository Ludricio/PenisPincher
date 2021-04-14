using Discord;

// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace PenisPincher.Discord
{
    public class AppConfig
    {
        public class DatabaseConfig
        {
            public string ConnectionString { get; private set; }
        }

        public class DiscordConfig
        {
            public string Token { get; private set; }

            public Color EmbedColor { get; private set; }

#pragma warning disable IDE0051 // Remove unused private members
            private uint EmbedColorHex
            {
                get => EmbedColor.RawValue;
                set => EmbedColor = new Color(value);
            }
#pragma warning restore IDE0051 // Remove unused private members
        }
    }
}