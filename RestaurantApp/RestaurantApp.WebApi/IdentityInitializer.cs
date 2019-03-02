using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            if (!roleManager.RoleExistsAsync("NormalUser").Result)
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
            if (userManager.FindByNameAsync("Admin").Result == null)
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = "Admin",
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
