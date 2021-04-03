using System;
using System.Collections.Generic;

namespace RestaurantApp.BusinessEntities.DTOs.Order
{
    public class PreviousOrderDetails
    {
        public DateTime SubmitDate { get; set; }

        public int Table { get; set; }

        public double Total { get; set; }

        public IEnumerable<OrderItemDetails> OrderItems { get; set; }
    }
}
