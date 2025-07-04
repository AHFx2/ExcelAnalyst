using ExcelAnalyst.Domain.Common.IRepositores;
using ExcelAnalyst.Domain.Global;
using ExcelAnalyst.Domain.Identity;
using ExcelAnalyst.Domain.Logger;
using ExcelAnalyst.Service.Objects.Users.DTOs;
using ExcelAnalyst.Service.Users.DTOs;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ExcelAnalyst.Service.Objects.Users
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<UserService> _logger;
        
        public UserService(UserManager<ApplicationUser> userManager, ILogger<UserService> logger)
        {
            _userManager = userManager;
            _logger = logger;
            
        }


        public async Task<Result<string>> AddUserAsync(AddUserDTO userDto)
        {
            if (userDto is null) 
            { 
                _logger.LogDebug("UserDto is null in AddUserAsync method.");
                return Result.Failure<string>(Error.General.NullValue); 
            }



            var existingUser = await _userManager.FindByNameAsync(userDto.UserName);

            if (existingUser != null)
            {
                _logger.LogDebug("User with username {UserName} already exists.", userDto.UserName);
                return Result.Failure<string>(Error.User.EmailAlreadyExists);
            }
            userDto.UserName = userDto.UserName.Trim();
            userDto.FirstName = userDto.FirstName.Trim();
            userDto.LastName = userDto.LastName.Trim();

            var user = userDto.Adapt<ApplicationUser>();
            var result = _userManager.CreateAsync(user, userDto.Password);


            if (result.Result.Succeeded)
            {
                _logger.LogInformation("User {UserName} created successfully.", user.UserName);
                return Result.Success(user.UserName);
            }

            else
            {
                string errorMessages = string.Join('\n', result.Result.Errors.Select(e => e.Description));
                _logger.LogCritical("Failed to create user: {Errors}", null, errorMessages);
                return Result.Failure<string>(new Error("User.CreationFailed", string.Join('\n', errorMessages)));
            }
        }

        public async Task<Result<List<UserDetailsDTO>>> GetAllUsersAsync()
        {
            var users = await _userManager.Users
                .ProjectToType<UserDetailsDTO>()
                .ToListAsync();

            return Result.Success(users);
        }
    }

}