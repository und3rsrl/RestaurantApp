using RestaurantApp.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantApp.WebApi.DTOs
{
    public class WaiterOrderInfoDTO
    {
        public int Id { get; set; }

        public string Submitter { get; set; }

        public double Total { get; set; }

        public int Table { get; set; }

        public bool WaiterPayment { get; set; }

        public IEnumerable<OrderItem> OrderItems { get; set; }
    }
}
