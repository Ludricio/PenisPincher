using PenisPincher.Persistence;

namespace PenisPincher.Core.Models
{
    public class RoleReactions : Entity<ulong>
    {
        public ulong MessageId { get; set; }
        public string EmoteName { get; set; }
    }
}