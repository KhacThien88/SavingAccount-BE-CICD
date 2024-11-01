using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SavingAccount_BE.Data;
using System;
using System.Threading.Tasks;

namespace SavingAccount_BE.Seeders
{
    public class UserSeeder
    {
        public static async Task InitializeUsers(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync("Admin"))
                {
                    await roleManager.CreateAsync(new IdentityRole("Admin"));
                }

                var adminEmail = "admin@gmail.com";
                var adminUser = await userManager.FindByEmailAsync(adminEmail);

                if (adminUser == null)
                {
                    adminUser = new ApplicationUser
                    {
                        UserName = adminEmail,
                        Email = adminEmail,
                        FullName = "Admin User",
                        BirthDate = new DateTime(1985, 5, 15),
                        Province = "Your Province",
                        City = "Your City",
                        Address = "Your Address",
                        Nation = "Your Nation"
                    };

                    var password = "111111aA@";
                    var createAdminResult = await userManager.CreateAsync(adminUser, password);

                    if (createAdminResult.Succeeded)
                    {
                        await userManager.AddToRoleAsync(adminUser, "Admin");
                    }
                }
            }
        }
    }
}
