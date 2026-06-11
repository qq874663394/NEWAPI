using Domain.Interface.IAggregateRoots;
using Domain.Interface.Repositories;
using Microsoft.EntityFrameworkCore;
using Repositories.DbContexts;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Repositories
{
    public class Repository<TEntity>
        : IRepository<TEntity>
        where TEntity : class, IEntity
    {
        protected readonly WebApiDbContext DbContext;

        protected readonly DbSet<TEntity> DbSet;

        public Repository(
            WebApiDbContext dbContext)
        {
            DbContext = dbContext;
            DbSet = dbContext.Set<TEntity>();
        }

        #region Query

        public virtual IQueryable<TEntity> Query()
        {
            return DbSet.AsNoTracking();
        }

        public virtual IQueryable<TEntity> Query(
            Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet
                .AsNoTracking()
                .Where(predicate);
        }

        public virtual async Task<TEntity?> GetByIdAsync(
            params object[] ids)
        {
            return await DbSet.FindAsync(ids);
        }

        public virtual async Task<TEntity?> FirstOrDefaultAsync(
            Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(predicate);
        }

        public virtual async Task<bool> AnyAsync(
            Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet.AnyAsync(predicate);
        }

        public virtual async Task<long> CountAsync(
            Expression<Func<TEntity, bool>>? predicate = null)
        {
            if (predicate == null)
                return await DbSet.LongCountAsync();

            return await DbSet.LongCountAsync(predicate);
        }

        public virtual async Task<List<TEntity>> ToListAsync()
        {
            return await DbSet
                .AsNoTracking()
                .ToListAsync();
        }

        public virtual async Task<List<TEntity>> ToListAsync(
            Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet
                .AsNoTracking()
                .Where(predicate)
                .ToListAsync();
        }

        #endregion

        #region Create

        public virtual async Task AddAsync(
            TEntity entity)
        {
            await DbSet.AddAsync(entity);
        }

        public virtual async Task AddRangeAsync(
            IEnumerable<TEntity> entities)
        {
            await DbSet.AddRangeAsync(entities);
        }

        #endregion

        #region Update

        public virtual void Update(
            TEntity entity)
        {
            DbSet.Update(entity);
        }

        public virtual void UpdateRange(
            IEnumerable<TEntity> entities)
        {
            DbSet.UpdateRange(entities);
        }

        #endregion

        #region Delete

        public virtual void Remove(
            TEntity entity)
        {
            DbSet.Remove(entity);
        }

        public virtual void RemoveRange(
            IEnumerable<TEntity> entities)
        {
            DbSet.RemoveRange(entities);
        }

        #endregion
    }
}
