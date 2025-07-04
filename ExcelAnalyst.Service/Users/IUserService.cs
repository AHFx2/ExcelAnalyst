using ExcelAnalyst.Service.Objects.Users.DTOs;
using ExcelAnalyst.Domain.Global;
using ExcelAnalyst.Service.Users.DTOs;

namespace ExcelAnalyst.Service.Objects.Users
{
    public interface IUserService
    {
        Task<Result<string>> AddUserAsync(AddUserDTO userDto);
        Task<Result<List<UserDetailsDTO>>> GetAllUsersAsync();
    }
}
