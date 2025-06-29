using System.Linq.Expressions;

namespace ExcelAnalyst.Domain.Common.IRepository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        #region Get
        Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> filter);
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? filter = null);
        IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> filter);
        IQueryable<TEntity> Table { get; }
        #endregion

        #region Add
        Task AddAsync(TEntity entity);
        Task AddRangeAsync(IEnumerable<TEntity> entities);
        #endregion

        #region Update
        Task UpdateAsync(TEntity entity);
        Task UpdateRangeAsync(IEnumerable<TEntity> entities);
        #endregion

        #region Delete
        Task DeleteAsync(TEntity entity);
        Task DeleteAsync(Expression<Func<TEntity, bool>> filter);
        Task DeleteRangeAsync(IEnumerable<TEntity> entities);
        #endregion
    }
}
