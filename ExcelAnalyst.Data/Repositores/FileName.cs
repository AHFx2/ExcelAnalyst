using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExcelAnalyst.Domain.Common.IRepositores;
using ExcelAnalyst.Domain.Global;
using ExcelAnalyst.Domain.Identity;
using ExcelAnalyst.Repository.EntityFrameworkCore.Context;
using Microsoft.EntityFrameworkCore;

namespace ExcelAnalyst.Data.Repositores
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<ApplicationUser> _dbSet;

        public UserRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Users;
        }
        public async Task<Result<ApplicationUser>> GetByUserNameAsync(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                return Result.Failure<ApplicationUser>(Error.General.NullValue);

            var result = await _dbSet.Include(x => x.RefreshTokens).Where(u => u.UserName == userName).FirstOrDefaultAsync();

            if (result is null)
                return Result.Failure<ApplicationUser>(Error.General.NoResult);

            return Result.Success(result);
        }
    }
}
