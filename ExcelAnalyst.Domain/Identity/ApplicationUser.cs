//using ExcelAnalyst.Domain.JWT;
using Microsoft.AspNetCore.Identity;

namespace ExcelAnalyst.Domain.Identity
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        //public List<RefreshToken> RefreshTokens { get; set; }
    }
}
