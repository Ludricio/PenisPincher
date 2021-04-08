using System.Collections.Generic;
using PenisPincher.Persistence;

namespace PenisPincher.Core.Models
{
    public class MonitoredStream : Entity<ulong>
    {
        public MonitoredStream(DiscordServerConfiguration owningServer)
        {
            OwningServer = owningServer;
        }

        public string StreamName { get; set; }

        public DiscordServerConfiguration OwningServer { get; set; }

        public string NotificationTemplate { get; set; }
        public List<ulong> NotificationRoleIds { get; set; }

    }
}