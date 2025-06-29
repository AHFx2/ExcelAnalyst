using System.ComponentModel.DataAnnotations.Schema;
using ExcelAnalyst.Domain.Common;
using ExcelAnalyst.Domain.Entities;
using ExcelAnalyst.Domain.Identity;
using Microsoft.EntityFrameworkCore;

namespace ExcelAnalyst.Domain.JWT
{
    public class RefreshToken : BaseEntity<int>
    {
        public string Token { get; set; }
        public DateTime ExpiresOn { get; set; }
        public bool IsExpired => DateTime.UtcNow >= ExpiresOn;
        public DateTime CreatedOn { get; set; }
        public DateTime? RevokedOn { get; set; }
        public bool IsActive => RevokedOn == null && !IsExpired;
        public int UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
