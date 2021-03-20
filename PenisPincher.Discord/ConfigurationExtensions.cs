using System;
using Microsoft.Extensions.Configuration;

namespace PenisPincher.Discord
{
    public static class ConfigurationExtensions
    {
        public static string DiscordToken(this IConfiguration config) => config["discord:token"];

        public static uint DiscordEmbedColor(this IConfiguration config)
        {
            return Convert.ToUInt32(config["discord:embed_color"]);
        }
    }
}
