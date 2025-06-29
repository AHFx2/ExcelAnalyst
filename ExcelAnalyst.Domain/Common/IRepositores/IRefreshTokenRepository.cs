using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExcelAnalyst.Domain.Global;
using ExcelAnalyst.Domain.JWT;

namespace ExcelAnalyst.Domain.Common.IRepositores
{
    public interface IRefreshTokenRepository
    {
        Task<Result<RefreshToken>> GetByTokenAsync(string token);
    }
}
