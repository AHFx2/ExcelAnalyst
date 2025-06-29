using ExcelAnalyst.Domain.Global;
using ExcelAnalyst.Domain.Identity;
using ExcelAnalyst.Domain.JWT;

namespace ExcelAnalyst.Service.Objects.JWT
{
  
    public interface IJwtService
    {
        Task<Result<AuthModel>> CreateTokenAsync(ApplicationUser user);
        Task<Result<AuthModel>> RefreshTokenAsync(string tokenString);
    }
}