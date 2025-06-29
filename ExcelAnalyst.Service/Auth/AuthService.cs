using ExcelAnalyst.Domain.Common.IRepositores;
using ExcelAnalyst.Domain.Common.IUnitOfWork;
using ExcelAnalyst.Domain.Global;
using ExcelAnalyst.Domain.Identity;
using ExcelAnalyst.Domain.JWT;
using ExcelAnalyst.Service.Objects.JWT;
using ExcelAnalyst.Service.Objects.Users.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ExcelAnalyst.Service.Objects.Auth
{
    public class AuthService : IAuthService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IJwtService _jwtService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        public AuthService(SignInManager<ApplicationUser> signInManager, IJwtService jwtService, IUnitOfWork unitOfWork, IUserRepository userRepository)
        {
            _signInManager = signInManager;
            _jwtService = jwtService;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
        }

        public async Task<Result<AuthModel>> LoginAsync(UserLoginDTO user)
        {
            if (user is null) return Result.Failure<AuthModel>(Error.General.NullValue);

            var existingUserResult = await _userRepository.GetByUserNameAsync(user.UserName);

            if (existingUserResult.IsFailure)
                return Result.Failure<AuthModel>(existingUserResult.Error);

            var existingUser = existingUserResult.Value;


            if (existingUser == null)
                return Result.Failure<AuthModel>(Error.User.InvalidCredentials);


            var result = await _signInManager.PasswordSignInAsync(existingUser, user.Password, true, true);
            if (result.IsLockedOut)
                return Result.Failure<AuthModel>(Error.User.AttempExceeded);


            if (!result.Succeeded)
                return Result.Failure<AuthModel>(Error.User.InvalidCredentials);

            var tokenResult = await _jwtService.CreateTokenAsync(existingUser);

            if (tokenResult.IsFailure)
                return Result.Failure<AuthModel>(tokenResult.Error);

            return Result.Success(tokenResult.Value);
        }

        public async Task<Result<AuthModel>> RefreshTokenAsync(string tokenString)
        {
            if (string.IsNullOrEmpty(tokenString)) return Result.Failure<AuthModel>(Error.General.NullValue);
            return await _jwtService.RefreshTokenAsync(tokenString);
        }
    }
}
