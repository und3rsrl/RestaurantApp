using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestaurantApp.WebApi.Models;

namespace RestaurantApp.WebApi.Entities
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {         
        }
        public DbSet<RestaurantApp.WebApi.Models.Categorie> Categories { get; set; }
    }
}
