using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PenisPincher.Persistence.Configuration
{
    public class EntityConfigurator<TEntity> where TEntity : class
    {
        private EntityTypeBuilder<TEntity> EntityTypeBuilder { get; }

        public EntityConfigurator(EntityTypeBuilder<TEntity> entityTypeBuilder)
        {
            EntityTypeBuilder = entityTypeBuilder;
        }

        public EntityConfigurator<TEntity> Has(Action<EntityTypeBuilder<TEntity>> builder)
        {
            builder(EntityTypeBuilder);
            return this;
        }
    }
}