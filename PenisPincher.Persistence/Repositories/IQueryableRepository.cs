using System.Linq;

namespace PenisPincher.Persistence.Repositories
{
    internal interface IQueryableRepository<out TEntity> where TEntity : class, IEntity<ulong>
    {
        internal IQueryable<TEntity> GetQueryable();
    }
}