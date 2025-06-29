using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExcelAnalyst.Domain.Identity;
using ExcelAnalyst.Repository.EntityFrameworkCore.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ExcelAnalyst.Data.Seeder
{
    public static class SeedData
    {
        public static class IdentitySeeder
        {
            public static async Task SeedAsync(IServiceProvider serviceProvider)
            {
                using var scope = serviceProvider.CreateScope();

                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                await context.Database.MigrateAsync();

                string adminEmail = "admin@example.com";
                string adminPassword = "Admin@123";


                if (await userManager.FindByEmailAsync(adminEmail) == null)
                {
                    var adminUser = new ApplicationUser
                    {
                        UserName = adminEmail,
                    };

                    var result = await userManager.CreateAsync(adminUser, adminPassword);
                }
            }
        }

    }
}
