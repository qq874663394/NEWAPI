using Domain.Base;
using System.Linq.Expressions;

namespace Domain.Interfaces.Repositories
{
    /// <summary>
    /// 通用仓储接口
    /// </summary>
    public interface IRepository<TEntity>
        where TEntity : class, IEntity
    {
        #region Query

        IQueryable<TEntity> Query();

        IQueryable<TEntity> Query(
            Expression<Func<TEntity, bool>> predicate);

        Task<TEntity?> GetByIdAsync(
            params object[] ids);

        Task<TEntity?> FirstOrDefaultAsync(
            Expression<Func<TEntity, bool>> predicate);

        Task<bool> AnyAsync(
            Expression<Func<TEntity, bool>> predicate);

        Task<long> CountAsync(
            Expression<Func<TEntity, bool>>? predicate = null);

        Task<List<TEntity>> ToListAsync();

        Task<List<TEntity>> ToListAsync(
            Expression<Func<TEntity, bool>> predicate);

        #endregion

        #region Create

        Task AddAsync(
            TEntity entity);

        Task AddRangeAsync(
            IEnumerable<TEntity> entities);

        #endregion

        #region Update

        void Update(
            TEntity entity);

        void UpdateRange(
            IEnumerable<TEntity> entities);

        #endregion

        #region Delete

        void Remove(
            TEntity entity);

        void RemoveRange(
            IEnumerable<TEntity> entities);

        #endregion
    }
}