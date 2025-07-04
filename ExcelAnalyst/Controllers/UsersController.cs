using System.Threading.Tasks;
using ExcelAnalyst.Service.Objects.Users;
using ExcelAnalyst.Service.Objects.Users.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExcelAnalyst.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("add")] 
        public async Task<IActionResult> AddAsync(AddUserDTO userDto)
        {
            if (userDto == null)
            {
                return BadRequest("User data cannot be null.");
            }
            var result = await _userService.AddUserAsync(userDto);
            if (result.IsFailure)
            {
                return BadRequest(new { Message = result.Error.Message });
            }
            return Ok(new { UserName = result.Value });
        }

        [HttpGet]
        [Route("getall")]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _userService.GetAllUsersAsync();
            if (result.IsFailure)
            {
                return BadRequest(new { Message = result.Error.Message });
            }
            return Ok(result.Value);
        }
    }
}
