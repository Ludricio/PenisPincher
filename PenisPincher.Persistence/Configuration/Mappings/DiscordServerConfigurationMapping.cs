using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PenisPincher.Core.Models;

namespace PenisPincher.Persistence.Configuration.Mappings
{
    internal class DiscordServerConfigurationMapping : EntityConfiguration<DiscordServerConfiguration>
    {
        protected override void Configure(EntityConfigurator<DiscordServerConfiguration> configurator)
        {
            configurator
                .Has(e => e.HasMany(p => p.MonitoredStreams).WithOne(p => p.OwningServer))
                .Has(e => e.HasMany(p => p.RoleReactions).WithOne(p => p.OwningServer))
                .Has(e => e.Property(p => p.ServerId).IsRequired())
                .Has(e => e.Property(p => p.LogLevel))
                .Has(e => e.Property(p => p.ServerLogChannelId))
                .Has(e => e.Property(p => p.StreamNotificationChannelId));
        }
    }
}
