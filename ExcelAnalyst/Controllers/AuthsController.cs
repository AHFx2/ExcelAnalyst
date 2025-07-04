using ExcelAnalyst.Service.Objects.Auth;
using ExcelAnalyst.Service.Objects.Users.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExcelAnalyst.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthsController : ControllerBase
    {


        private readonly IAuthService _userService;
        public AuthsController(IAuthService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<ActionResult> LoginAsync(UserLoginDTO userLogin)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToArray();
                return BadRequest("Login Faild due: " + string.Join('\n', errors));
            }

            var result = await _userService.LoginAsync(userLogin);

            if (result.IsFailure)
            {
                return BadRequest(new { message = result.Error.Message });
            }

            SetRefreshTokenInCookie(result.Value.RefreshToken, result.Value.RefreshTokenExpiration);

            return Ok(new { token = result.Value });

        }

        [HttpPost("auth/logout")]
        public async Task<IActionResult> Logout()
        {
            Response.Cookies.Delete("refreshToken");
            return Ok();
        }


        [HttpGet("refreshtoken")]
        [AllowAnonymous]
        public async Task<ActionResult> RefreshTokenAsync()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshToken))
            {
                return Unauthorized(new { Message = "Refresh token is missing." });
            }
            var result = await _userService.RefreshTokenAsync(refreshToken);

            if (result.IsFailure)
            {
                return BadRequest(new { Message = result.Error.Message });
            }

            SetRefreshTokenInCookie(result.Value.RefreshToken, result.Value.RefreshTokenExpiration);
            return Ok(new { token = result.Value });
        }

        private void SetRefreshTokenInCookie(string refreshToken, DateTime expires)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = expires.ToLocalTime(),
                Secure = true,
                IsEssential = true,
                SameSite = SameSiteMode.None
            };

            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }
    }
}
