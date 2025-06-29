using Microsoft.EntityFrameworkCore.Storage;
using ExcelAnalyst.Domain.Common.IRepository;
using ExcelAnalyst.Domain.Common.IUnitOfWork;
using ExcelAnalyst.Domain.Global;
using ExcelAnalyst  .Repository.EntityFrameworkCore.Context;
using Academy.Repository.Repository;

namespace ExcelAnalyst.Repository.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IDbContextTransaction? _currentTransaction;

        public UnitOfWork(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _repositories = new Dictionary<Type, object>();
        }

        private readonly Dictionary<Type, object> _repositories;
        public IRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            if (!_repositories.TryGetValue(typeof(TEntity), out var repository))
            {
                repository = new Repository<TEntity>(_context);
                _repositories[typeof(TEntity)] = repository;
            }

            return (IRepository<TEntity>)repository;
        }
        public async Task<Result> SaveChangesAsync()
        {
            try
            {
                var changes = await _context.SaveChangesAsync();
                return changes > 0
                    ? Result.Success()
                    : Result.Failure(Error.EFCore.NoChanges);
            }
            catch (Exception ex)
            {
                return Result.Failure(new Error ("EFCore.Unoknown", $"An error occurred while saving changes: {ex.Message}" ));
            }
        }
        public async Task<Result> StartTransactionAsync()
        {
            try
            {
                if (_currentTransaction != null)
                    return Result.Failure(new Error ("EFCore.Unknown", "A transaction is already in progress." ));

                _currentTransaction = await _context.Database.BeginTransactionAsync();
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(new Error("EFCore.Unoknown", $"Failed to start a transaction: {ex.Message}"));
            }
        }
        public async Task<Result> CommitAsync()
        {
            try
            {
                if (_currentTransaction == null)
                    return Result.Failure(new Error("EFCore.Unoknown", "No active transaction to commit."));

                var saveResult = await SaveChangesAsync();
                if (saveResult.IsFailure)
                {
                    return Result.Failure(new Error("EFCore.Unoknown", $"An error occurred during commit: {saveResult.Error.Message}"));
                }
                await _currentTransaction.CommitAsync();
                CleanupTransaction();
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(new Error("EFCore.Unoknown", $"An error occurred during commit: {ex.Message}"));
            }
        }
        public async Task<Result> RollbackAsync()
        {
            try
            {
                if (_currentTransaction == null)
                    return Result.Failure(new Error("EFCore.Unoknown", "No active transaction to roll back."));

                await _currentTransaction.RollbackAsync();
                CleanupTransaction();
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(new Error("EFCore.Unoknown", $"An error occurred during rollback: {ex.Message}"));
            }
        }
        private void CleanupTransaction()
        {
            _currentTransaction?.Dispose();
            _currentTransaction = null;
        }
        public void Dispose()
        {
            try
            {
                _context.Dispose();
                _currentTransaction?.Dispose();
            }
            catch (Exception ex)
            {
                // Handle dispose exception if needed
                throw new Exception($"An error occurred during disposal: {ex.Message}");
            }
        }
    }
}