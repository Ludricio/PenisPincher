using Microsoft.EntityFrameworkCore;
using PenisPincher.Core.Models;

namespace PenisPincher.Persistence.Configuration.Mappings
{
    internal class ReactionRoleMapping : EntityConfiguration<ReactionRole>
    {
        protected override void Configure(EntityConfigurator<ReactionRole> configurator)
        {
            configurator
                .Has(e => e.HasOne(p => p.OwningServer).WithMany(p => p.RoleReactions).OnDelete(DeleteBehavior.Cascade))
                .Has(e => e.Property(p => p.EmoteName).IsRequired())
                .Has(e => e.Property(p => p.RoleId).IsRequired())
                .Has(e => e.Property(p => p.MessageId).IsRequired())
                .Has(e => e.Property(p => p.ChannelId).IsRequired());
        }
    }
}