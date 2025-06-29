using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExcelAnalyst.Domain.Common.IRepositores;
using ExcelAnalyst.Domain.Global;
using ExcelAnalyst.Domain.JWT;
using ExcelAnalyst.Repository.EntityFrameworkCore.Context;
using Microsoft.EntityFrameworkCore;

namespace ExcelAnalyst.Data.Repositores
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<RefreshToken> _dbSet;

        public RefreshTokenRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.RefreshTokens;
        }
        public async Task<Result<RefreshToken>> GetByTokenAsync(string token)
        {
            if (string.IsNullOrEmpty(token))
                return Result.Failure<RefreshToken>(Error.General.NullValue);

            var result = await _dbSet.Include(x => x.User).Where(t => t.Token == token).FirstOrDefaultAsync();

            if (result is null)
                return Result.Failure<RefreshToken>(Error.General.NoResult);

            return Result.Success(result);
        }
    }
}
