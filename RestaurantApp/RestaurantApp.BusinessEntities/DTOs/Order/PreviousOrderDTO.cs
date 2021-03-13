using System;
using System.Collections.Generic;

namespace RestaurantApp.BusinessEntities.DTOs.Order
{
    public class PreviousOrderDTO
    {
        public DateTime SubmitDate { get; set; }

        public int Table { get; set; }

        public double Total { get; set; }

        public IEnumerable<OrderItemDTO> OrderItems { get; set; }
    }
}
