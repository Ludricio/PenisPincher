using System.Collections.Generic;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using PenisPincher.Core.Models;

namespace PenisPincher.Persistence.Configuration.Mappings
{
    internal class MonitoredStreamMapping : EntityConfiguration<MonitoredStream>
    {
        protected override void Configure(EntityConfigurator<MonitoredStream> configurator)
        {
            configurator
                .Has(e => e.HasOne(p => p.OwningServer).WithMany(p => p.MonitoredStreams)
                    .OnDelete(DeleteBehavior.Cascade))
                .Has(e => e.Property(p => p.NotificationTemplate).IsRequired())
                .Has(e => e.Property(p => p.StreamName))
                .Has(e => e.Property(p => p.NotificationRoleIds).HasConversion(
                    v => JsonSerializer.Serialize(v, null),
                    v => JsonSerializer.Deserialize<List<ulong>>(v, null)));
        }
    }
}
