using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ExcelAnalyst.Domain.JWT
{
    public class AuthModel
    {
        public string UserName { get; set; }
        public int UserId { get; set; }
        public string AccessToken { get; set; }

        [JsonIgnore]
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiration { get; set; }
    }
}
