using PenisPincher.Persistence;

namespace PenisPincher.Core.Models
{
    public class RoleReaction : Entity<ulong>
    {
        public RoleReaction(DiscordServerConfiguration owningServer)
        {
            OwningServer = owningServer;
        }

        public ulong MessageId { get; set; }
        public string EmoteName { get; set; }

        public DiscordServerConfiguration OwningServer { get; set; }
    }
}