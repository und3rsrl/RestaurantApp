namespace RestaurantApp.API.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using RestaurantApp.API.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<RestaurantApp.API.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "RestaurantApp.API.Models.ApplicationDbContext";
        }

        protected override void Seed(RestaurantApp.API.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            using (var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context)))
            {
                using (var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context)))
                {
                    if (!roleManager.RoleExists("Admin"))
                    {
                        roleManager.Create(new IdentityRole("Admin"));
                    }

                    if (!roleManager.RoleExists("Waiter"))
                    {
                        roleManager.Create(new IdentityRole("Waiter"));
                    }

                    if (!roleManager.RoleExists("User"))
                    {
                        roleManager.Create(new IdentityRole("User"));
                    }

                    var adminUser = new ApplicationUser()
                    {
                        UserName = "admin@gmail.com",
                    };

                    if (userManager.FindByName("admin@gmail.com") == null)
                    {
                        userManager.Create(adminUser, "@L3x2005");
                            userManager.AddToRole(adminUser.Id, "Admin");
                    }

                    context.SaveChanges();
                }
            }
        }
    }
}
