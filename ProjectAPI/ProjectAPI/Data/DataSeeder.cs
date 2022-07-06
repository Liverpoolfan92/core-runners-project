using Microsoft.AspNetCore.Identity;
using ProjectAPI.Context;
using ProjectAPI.Data.Models;

namespace ProjectAPI.Data
{
    public static class DataSeeder
    {
        public static void Seed(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var AdminRole = new IdentityRole("Admin");
                var UserRole = new IdentityRole("User");
                var Admin = new User();

                roleManager.CreateAsync(AdminRole);
                roleManager.CreateAsync(UserRole);
                userManager.CreateAsync(Admin);

            };
      

        }

    }
}

