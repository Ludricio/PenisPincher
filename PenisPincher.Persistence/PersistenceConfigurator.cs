using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PenisPincher.Utilities.Extensions;

namespace PenisPincher.Persistence
{
    public static class PersistenceConfigurator
    {
        public static IServiceCollection AddPersistence(this IServiceCollection @this, string connString)
        {
            connString.ThrowIfNull(nameof(connString));

            return @this
                .AddDbContext<DbContext, PenisPincherDbContext>(); //TODO Add options that add sql server
            //TODO add configurations
        }
    }
}
