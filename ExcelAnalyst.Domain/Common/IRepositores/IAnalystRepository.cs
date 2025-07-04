using ExcelAnalyst.Domain.Entities;

namespace ExcelAnalyst.Domain.Common.IRepositores
{
    public interface IAnalystRepository
    {
        Task<bool> IsExsistAsync(string department, DateTime date);
        Task<bool> AddAsync(Analyst analyst);
    }
}
