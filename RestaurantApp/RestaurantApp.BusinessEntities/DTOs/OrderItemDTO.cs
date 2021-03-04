using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantApp.WebApi.DTOs
{
    public class OrderItemDTO
    {
        public int ProductId { get; set; }

        public string Name { get; set; }

        public int Amount { get; set; }

        public double Price { get; set; }

        public double Total { get; set; }
    }
}
