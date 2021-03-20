using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace PenisPincher.Discord
{

    public class DiscordLauncher
    {
        private ILogger<DiscordLauncher> Logger { get; set; }
        private ManualResetEvent WaitHandle { get; }
        public DiscordLauncher(ILogger<DiscordLauncher> logger)
        {
            Logger = logger;
            WaitHandle = new ManualResetEvent(false);
        }

        public async Task StartDiscordServicesAsync(IServiceProvider serviceProvider)
        {

        }
    }
}