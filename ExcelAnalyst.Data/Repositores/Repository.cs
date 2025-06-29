using System.Linq.Expressions;
using ExcelAnalyst.Domain.Common.IRepository;
using ExcelAnalyst.Repository.EntityFrameworkCore.Context;
using Microsoft.EntityFrameworkCore;

namespace Academy.Repository.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly AppDbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public Repository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public IQueryable<TEntity> Table => _dbSet;

        #region Asynchronus Adding
        public async Task AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }


        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        #endregion

        #region Asynchronus Deleting
        public async Task DeleteAsync(TEntity entity)
        {
            await Task.Run(() => _dbSet.Remove(entity));
        }

        public async Task DeleteAsync(Expression<Func<TEntity, bool>> filter)
        {
            await Task.Run(() =>
            {
                var entities = _dbSet.Where(filter);
                _dbSet.RemoveRange(entities);
            });
        }

        public async Task DeleteRangeAsync(IEnumerable<TEntity> entities)
        {
            await Task.Run(() => _dbSet.RemoveRange(entities));
        }

        #endregion

        #region  Asynchronus Getting
        public IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> filter)
        {
            return _dbSet.Where(filter);
        }


        public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? filter = null)
        {
            return await (filter == null ? _dbSet.ToListAsync() : _dbSet.Where(filter).ToListAsync());
        }

        public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await _dbSet.FirstOrDefaultAsync(filter);
        }
        #endregion

        #region Asynchronus Updating
        public async Task UpdateAsync(TEntity entity)
        {
            await Task.Run(() => _dbSet.Update(entity));
        }

        public async Task UpdateRangeAsync(IEnumerable<TEntity> entities)
        {
            await Task.Run(() => _dbSet.UpdateRange(entities));
        }
        #endregion

    }
}