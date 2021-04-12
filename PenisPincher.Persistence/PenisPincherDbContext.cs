using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PenisPincher.Persistence.Configuration.Mappings;

namespace PenisPincher.Persistence
{
    public class PenisPincherDbContext : DbContext
    {
        public PenisPincherDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .ApplyConfiguration(new DiscordServerConfigurationMapping());
            base.OnModelCreating(modelBuilder);
        }
    }
}
