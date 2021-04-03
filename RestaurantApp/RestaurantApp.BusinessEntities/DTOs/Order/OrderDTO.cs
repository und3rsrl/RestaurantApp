using System;
using System.Collections.Generic;

namespace RestaurantApp.BusinessEntities.DTOs.Order
{
    public class OrderDTO
    {
        public string Submitter { get; set; }

        public DateTime SubmiteDatetime { get; set; }

        public int Table { get; set; }

        public double Total { get; set; }

        public ICollection<OrderItemDetails> OrderItems { get; set; }
    }
}
