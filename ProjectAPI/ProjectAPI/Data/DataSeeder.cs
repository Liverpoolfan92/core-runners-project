using Microsoft.AspNetCore.Identity;
using ProjectAPI.Context;
using ProjectAPI.Data.Models;

namespace ProjectAPI.Data
{
    public static class DataSeeder
    {
        public static async Task Seed(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                if (!roleManager.Roles.Any())
                {
                    var AdminRole = new IdentityRole("Admin");
                    var UserRole = new IdentityRole("User");
                    await roleManager.CreateAsync(AdminRole);
                    await roleManager.CreateAsync(UserRole);
                }

                if (!userManager.Users.Any())
                {
                    var Admin = new User("Admin");
                    Admin.Email = "admin@mail.bg";
                    Admin.NormalizedEmail = Admin.Email.Normalize().ToUpper();

                    await userManager.CreateAsync(Admin, "P@ssw0rd123");

                    await userManager.AddToRoleAsync(Admin, "Admin");
                }

            };
      

        }

    }
}

