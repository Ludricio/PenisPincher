using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PenisPincher.Core;

namespace PenisPincher.Persistence.Configuration
{
    public abstract class EntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : class, IEntity
    {
        public void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.CreatedBy).IsRequired();
            builder.Property(p => p.CreatedDate).IsRequired();
            builder.Property(p => p.ModifiedBy).IsRequired();
            builder.Property(p => p.ModifiedDate).IsRequired();
            builder.Property(p => p.Version).IsRequired();
            Configure(new EntityConfigurator<TEntity>(builder));
        }

        protected abstract void Configure(EntityConfigurator<TEntity> configurator);
    }
}
