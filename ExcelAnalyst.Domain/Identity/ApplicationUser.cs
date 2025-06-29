using ExcelAnalyst.Domain.JWT;
using Microsoft.AspNetCore.Identity;

namespace ExcelAnalyst.Domain.Identity
{
    public class ApplicationUser : IdentityUser<int>
    {
        public List<RefreshToken> RefreshTokens { get; set; }
    }
}
