using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PenisPincher.Persistence.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity>, IQueryableRepository<TEntity> where TEntity : class, IEntity<ulong>
    {
        protected DbContext Context { get; }

        public Repository(DbContext context)
        {
            Context = context;
        }

        public IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> predicate = null)
        {
            return predicate is not null 
                ? Context.Set<TEntity>().Where(predicate) 
                : Context.Set<TEntity>();
        }

        public virtual TEntity Get(ulong id)
        {
            return Context.Find<TEntity>(id);
        }

        public async IAsyncEnumerable<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate = null)
        {
            if(predicate is not null)
            {
                await foreach(var entity in Context.Set<TEntity>())
                {
                    yield return entity;
                }
            }
            await foreach(var entity in Context.Set<TEntity>())
            {
                yield return entity;
            }
        }

        public virtual async Task<TEntity> GetAsync(ulong id)
        {
            return await Context.FindAsync<TEntity>(id);
        }

        public virtual void Add(TEntity entity)
        {
            Context.Add(entity);
        }

        public virtual void AddRange(IEnumerable<TEntity> entities)
        {
            Context.AddRange(entities);
        }

        public virtual async Task AddAsync(TEntity entity)
        {
            await Context.AddAsync(entity);
        }

        public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await Context.AddRangeAsync(entities);
        }

        public virtual void Update(TEntity entity)
        {
            Context.Update(entity);
        }

        public virtual void UpdateRange(IEnumerable<TEntity> entities)
        {
            Context.UpdateRange(entities);
        }

        public virtual void Remove(TEntity entity)
        {
            Context.Remove(entity);
        }

        public virtual void RemoveRange(IEnumerable<TEntity> entities)
        {
            Context.RemoveRange(entities);
        }

        public virtual int Count(Expression<Func<TEntity, bool>> predicate = null)
        {
            return predicate is not null
                ? Context.Set<TEntity>().Count(predicate)
                : Context.Set<TEntity>().Count();
        }

        public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate = null)
        {
            return predicate is not null
                ? await Context.Set<TEntity>().CountAsync(predicate)
                : await Context.Set<TEntity>().CountAsync();
        }

        IQueryable<TEntity> IQueryableRepository<TEntity>.GetQueryable()
        {
            return Context.Set<TEntity>();
        }
    }
}