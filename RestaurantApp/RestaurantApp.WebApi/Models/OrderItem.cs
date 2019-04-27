using System.ComponentModel.DataAnnotations;

namespace RestaurantApp.WebApi.Models
{
    public class OrderItem
    {
        [Key]
        public int OrderItemId { get; set; }

        public int ProductId { get; set; }

        public string Name { get; set; }

        public int Amount { get; set; }

        public double Price { get; set; }

        public double Total { get; set; }
    }
}