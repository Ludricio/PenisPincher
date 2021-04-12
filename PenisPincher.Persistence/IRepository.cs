using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using PenisPincher.Core;

namespace PenisPincher.Persistence
{
    public interface IRepository<TEntity> where TEntity : class, IEntity<ulong>
    {


        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> predicate = null);
        TEntity Get(ulong id);
        IAsyncEnumerable<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate = null);
        Task<TEntity> GetAsync(ulong id);

        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);
        Task AddAsync(TEntity entity);
        Task AddRangeAsync(IEnumerable<TEntity> entities);

        void Update(TEntity entity);
        void UpdateRange(IEnumerable<TEntity> entities);

        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);

        int Count(Expression<Func<TEntity, bool>> predicate = null);
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate = null);
    }
}
