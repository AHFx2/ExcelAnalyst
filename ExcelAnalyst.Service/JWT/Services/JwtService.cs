using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ExcelAnalyst.Domain.Common.IRepositores;
using ExcelAnalyst.Domain.Common.IUnitOfWork;
using ExcelAnalyst.Domain.Global;
using ExcelAnalyst.Domain.Identity;
using ExcelAnalyst.Domain.JWT;
using ExcelAnalyst.Domain.JWTOptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;


namespace ExcelAnalyst.Service.Objects.JWT
{
    public class JwtService : IJwtService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly JWToptions _JWToptions;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        public JwtService(JWToptions jWToptions, UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork, IRefreshTokenRepository refreshTokenRepository)
        {
            _JWToptions = jWToptions;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _refreshTokenRepository = refreshTokenRepository;

        }

        private List<Claim> GenerateUserClaims(string userName, int userId)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            };

            return claims;
        }

        public SecurityToken GenerateToken(string userName, int userId)
        {
            var claims = GenerateUserClaims(userName, userId);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _JWToptions.Issuer,
                Audience = _JWToptions.Audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_JWToptions.SigningKey)),
                    SecurityAlgorithms.HmacSha256),
                Expires = DateTime.Now.AddMinutes(_JWToptions.Lifetime),
                Subject = new ClaimsIdentity(claims)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return token;
        }

        public string GenerateTokenString(string userName, int userId)
        {
            var token = GenerateToken(userName, userId);
            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }


        public async Task<Result<AuthModel>> CreateTokenAsync(ApplicationUser user)
        {
            if (user is null) return Result.Failure<AuthModel>(Error.General.FailedOperation);

            var tokenString = GenerateTokenString(user.UserName, user.Id);
            
            var token = new AuthModel
            {
                UserId = user.Id,
                UserName = user.UserName,
                AccessToken = tokenString
            };


            if (user.RefreshTokens.Any(x => x.IsActive))
            {
                var activeRefreshToken = user.RefreshTokens.FirstOrDefault(t => t.IsActive);
                token.RefreshToken = activeRefreshToken.Token;
                token.RefreshTokenExpiration = activeRefreshToken.ExpiresOn;
            }
            else
            {
                var refreshToken = GenerateRefreshToken();
                token.RefreshToken = refreshToken.Token;
                token.RefreshTokenExpiration = refreshToken.ExpiresOn;
                user.RefreshTokens.Add(refreshToken);
                await _userManager.UpdateAsync(user);
                await _unitOfWork.SaveChangesAsync();
            }

            return Result.Success(token);
        }
        public RefreshToken GenerateRefreshToken()
        {
            var randomNumber = new byte[32];

            using var generator = new RNGCryptoServiceProvider();

            generator.GetBytes(randomNumber);

            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomNumber),
                ExpiresOn = DateTime.UtcNow.AddDays(1),
                CreatedOn = DateTime.UtcNow
            };
        }


        public async Task<Result<AuthModel>> RefreshTokenAsync(string tokenString)
        {

            var refreshTokenResult = await _refreshTokenRepository.GetByTokenAsync(tokenString);

            if (refreshTokenResult.IsFailure)
                return Result.Failure<AuthModel>(refreshTokenResult.Error);

            var refreshToken = refreshTokenResult.Value;


            if (refreshToken is null || refreshToken.User == null || !refreshToken.IsActive)
            {
                return Result.Failure<AuthModel>(Error.General.FailedOperation);
            }


            refreshToken.RevokedOn = DateTime.UtcNow;

            var newRefreshToken = GenerateRefreshToken();
            refreshToken.User.RefreshTokens.Add(newRefreshToken);
            await _userManager.UpdateAsync(refreshToken.User);

            var token = new AuthModel  {
                UserId = refreshToken.User.Id,
                AccessToken = GenerateTokenString(refreshToken.User.UserName, refreshToken.User.Id),
                UserName = refreshToken.User.UserName,
                RefreshToken = newRefreshToken.Token,
                RefreshTokenExpiration = newRefreshToken.ExpiresOn
            };

            return Result.Success(token);
        }
    }
}
