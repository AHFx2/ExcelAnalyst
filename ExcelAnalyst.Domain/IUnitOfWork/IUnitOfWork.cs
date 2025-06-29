using ExcelAnalyst.Domain.Global;

namespace ExcelAnalyst.Domain.Common.IUnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {

        Task<Result> SaveChangesAsync();

        Task<Result> StartTransactionAsync();

        Task<Result> CommitAsync();

        Task<Result> RollbackAsync();
    }
}
