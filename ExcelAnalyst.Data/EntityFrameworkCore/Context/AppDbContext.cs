using ExcelAnalyst.Domain.Entities;
using ExcelAnalyst.Domain.Identity;
using ExcelAnalyst.Domain.JWT;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace ExcelAnalyst.Repository.EntityFrameworkCore.Context
{

    public class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
    {
        public DbSet<Analyst> Analytics { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public AppDbContext()  { }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);         
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            modelBuilder.Entity<ApplicationUser>(entity => entity.ToTable("Users", "Security"));
            modelBuilder.Entity<ApplicationRole>(entity => entity.ToTable("Roles", "Security"));
            modelBuilder.Entity<IdentityUserRole<int>>(entity => entity.ToTable("UserRoles", "Security"));
            modelBuilder.Entity<IdentityUserClaim<int>>(entity => entity.ToTable("UserClaims", "Security"));
            modelBuilder.Entity<IdentityUserLogin<int>>(entity => entity.ToTable("UserLogins", "Security"));
            modelBuilder.Entity<IdentityRoleClaim<int>>(entity => entity.ToTable("RoleClaims", "Security"));
            modelBuilder.Entity<IdentityUserToken<int>>(entity => entity.ToTable("UserTokens", "Security"));
        }
    }
}