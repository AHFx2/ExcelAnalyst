using ExcelAnalyst.Domain.Global;
using ExcelAnalyst.Domain.JWT;
using ExcelAnalyst.Service.Objects.Users.DTOs;

namespace ExcelAnalyst.Service.Objects.Auth
{
    public interface IAuthService
    {
        Task<Result<AuthModel>> LoginAsync(UserLoginDTO user);
        Task<Result<AuthModel>> RefreshTokenAsync(string tokenString);
        
    }
}