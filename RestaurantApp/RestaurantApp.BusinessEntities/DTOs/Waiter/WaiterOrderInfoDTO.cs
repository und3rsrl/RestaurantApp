using RestaurantApp.BusinessEntities.DTOs.Order;
using System.Collections.Generic;

namespace RestaurantApp.BusinessEntities.DTOs.Waiter
{
    public class WaiterOrderInfoDTO
    {
        public int Id { get; set; }

        public string Submitter { get; set; }

        public double Total { get; set; }

        public int Table { get; set; }

        public bool WaiterPayment { get; set; }

        public IEnumerable<OrderItemDTO> OrderItems { get; set; }
    }
}
