using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExcelAnalyst.Domain.Global;
using ExcelAnalyst.Domain.Identity;

namespace ExcelAnalyst.Domain.Common.IRepositores
{
    public interface IUserRepository
    {
        Task<Result<ApplicationUser>> GetByUserNameAsync(string userName);
    
    }
}
