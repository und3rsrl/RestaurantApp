using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantApp.Models
{
    public class WaiterOrderInfo
    {
        public int Id { get; set; }

        public string Submitter { get; set; }

        public double Total { get; set; }

        public int Table { get; set; }

        public bool WaiterPayment { get; set; }

        public IEnumerable<OrderItem> OrderItems { get; set; }
    }
}
