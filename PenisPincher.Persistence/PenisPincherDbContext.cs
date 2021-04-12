using Microsoft.EntityFrameworkCore;

namespace PenisPincher.Persistence
{
    public class PenisPincherDbContext : DbContext
    {
        public PenisPincherDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PenisPincherDbContext).Assembly);
        }
    }
}