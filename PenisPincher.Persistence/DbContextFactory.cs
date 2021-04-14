using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace PenisPincher.Persistence
{
    public class DbContextFactory : IDesignTimeDbContextFactory<PenisPincherDbContext>
    {
        public PenisPincherDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddYamlFile("dbconfig.yaml")
                .Build();
            var builder = new DbContextOptionsBuilder<PenisPincherDbContext>();
            builder.UseSqlServer(configuration["database:connection_string"]);
            return new PenisPincherDbContext(builder.Options);
        }
    }
}