using RestaurantApp.BusinessEntities.DTOs.Order;
using System.Collections.Generic;

namespace RestaurantApp.BusinessEntities.DTOs.Waiter
{
    public class WaiterOrderInfoDetails
    {
        public int Id { get; set; }

        public string Submitter { get; set; }

        public double Total { get; set; }

        public int Table { get; set; }

        public bool WaiterPayment { get; set; }

        public IEnumerable<OrderItemDetails> OrderItems { get; set; }
    }
}
