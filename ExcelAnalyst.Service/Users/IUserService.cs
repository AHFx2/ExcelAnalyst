using ExcelAnalyst.Service.Objects.Users.DTOs;
using ExcelAnalyst.Domain.Global;

namespace ExcelAnalyst.Service.Objects.Users
{
    public interface IUserService
    {
        Task<Result<int>> AddUserAsync(AddUserDTO userDto);

    }
}
