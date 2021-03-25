using PenisPincher.Persistence;

namespace PenisPincher.Core.Models
{
    public class RoleReactions : Entity<ulong>
    {
        public RoleReactions(DiscordServerConfiguration owningServer)
        {
            OwningServer = owningServer;
        }

        public ulong MessageId { get; set; }
        public string EmoteName { get; set; }

        public DiscordServerConfiguration OwningServer { get; set; }
    }
}