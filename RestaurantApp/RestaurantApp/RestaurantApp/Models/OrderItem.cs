using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantApp.Models
{
    public class OrderItem
    {
        public int ProductId { get; set; }

        public int Amount { get; set; }

        public double Price { get; set; }

        public double Total { get; set; }
    }
}
