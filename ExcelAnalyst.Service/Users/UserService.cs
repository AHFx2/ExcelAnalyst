using ExcelAnalyst.Domain.Common.IRepositores;
using ExcelAnalyst.Domain.Global;
using ExcelAnalyst.Domain.Identity;
using ExcelAnalyst.Service.Objects.Users.DTOs;
using Mapster;
using Microsoft.AspNetCore.Identity;

namespace ExcelAnalyst.Service.Objects.Users
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }


        public async Task<Result<int>> AddUserAsync(AddUserDTO userDto)
        {
            if (userDto is null) return Result.Failure<int>(Error.General.NullValue);

            var existingUser = await _userManager.FindByNameAsync(userDto.UserName);

            if (existingUser != null)
            {
                return Result.Failure<int>(Error.User.EmailAlreadyExists);
            }


            var user = userDto.Adapt<ApplicationUser>();
            var result = _userManager.CreateAsync(user, userDto.Password);


            if (result.Result.Succeeded)
            {
                return Result.Success(user.Id);
            }

            else
            {
                return Result.Failure<int>(new Error("User.CreationFailed", string.Join('\n', result.Result.Errors.Select(v => v.Description))));
            }
        }


    }

}