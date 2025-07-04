using ExcelAnalyst.Domain.Common.IRepositores;
using ExcelAnalyst.Domain.Entities;
using ExcelAnalyst.Domain.Global;
using ExcelAnalyst.Domain.Identity;
using ExcelAnalyst.Repository.EntityFrameworkCore.Context;
using Microsoft.EntityFrameworkCore;

namespace ExcelAnalyst.Data.Repositores
{
    public class AnalystRepository : IAnalystRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<Analyst> _dbSet;

        public AnalystRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Analytics;
        }

        public async Task<bool> AddAsync(Analyst analyst)
        {
            if (analyst == null || string.IsNullOrEmpty(analyst.Department) || analyst.Date == default)
                return false;
            _dbSet.Add(analyst);
            return (await _context.SaveChangesAsync()) > 0;
        }

        public async Task<bool> IsExsistAsync(string department, DateTime date)
        {
            if (string.IsNullOrEmpty(department) || date == default)
                return false;
            return await _dbSet.AnyAsync(x => x.Department == department && x.Date.Date == date.Date);
        }
    }
}
