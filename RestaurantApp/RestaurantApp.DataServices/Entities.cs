using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RestaurantApp.DataModel.Models;

namespace RestaurantApp.DataServices
{
    public class Entities : IdentityDbContext
    {
        public Entities(DbContextOptions<Entities> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Food> Foods { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<WaiterStatus> WaitersStatus { get; set; }
        public DbSet<ForgotPassword> ForgotPassword { get; set; }
    }
}
