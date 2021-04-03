using Microsoft.AspNetCore.Identity;

namespace RestaurantApp.WebApi
{
    public static class IdentityInitializer
    {
        public static void SeedData(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);
        }

        public static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("User").Result)
            {
                IdentityRole role = new IdentityRole
                {
                    Name = "User"
                };
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }

            if (!roleManager.RoleExistsAsync("Waiter").Result)
            {
                IdentityRole role = new IdentityRole
                {
                    Name = "Waiter"
                };
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }

            if (!roleManager.RoleExistsAsync("Administrator").Result)
            {
                IdentityRole role = new IdentityRole
                {
                    Name = "Administrator"
                };
                IdentityResult roleResult = roleManager.
                CreateAsync(role).Result;
            }
        }

        public static void SeedUsers(UserManager<IdentityUser> userManager)
        {
            if (userManager.FindByNameAsync("admin@restaurant.com").Result == null)
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = "admin@restaurant.com",
                    Email = "admin@restaurant.com"
                };

                IdentityResult result = userManager.CreateAsync(user, "@L3x2005").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Administrator").Wait();
                }
            }
        }
    }
}
