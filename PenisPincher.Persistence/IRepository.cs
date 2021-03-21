using System;
using System.Linq.Expressions;

namespace PenisPincher.Persistence
{
    public interface IRepository<TEntity> where TEntity : class
    {

        TEntity Get(ulong id);

        void Insert(TEntity entity);

        void Update(TEntity entity);

        void Remove(ulong id);

        int Count(Expression<Func<TEntity, bool>> predicate = null);
    }
}
