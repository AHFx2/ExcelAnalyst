using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ExcelAnalyst.Service.Users.DTOs
{
    public class UserDetailsDTO
    {
        public string UserName { get; set; }
        [JsonIgnore]
        public string FirstName { get; set; }
        [JsonIgnore]
        public string LastName { get; set; }
        public string FullName => FirstName + " " + LastName; 
        public string Role { get; set; }
    }
}
