namespace PenisPincher.Core.Models
{
    public class ReactionRole : Entity<ulong>
    {
        public ReactionRole(DiscordServerConfiguration owningServer)
        {
            OwningServer = owningServer;
        }

        private ReactionRole()
        {
        }

        public ulong MessageId { get; set; }
        public string EmoteName { get; set; }
        public ulong RoleId { get; set; }
        public ulong ChannelId { get; set; }

        public DiscordServerConfiguration OwningServer { get; set; }
    }
}