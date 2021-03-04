using System;
using System.Collections.Generic;

namespace RestaurantApp.DataModel.Models
{
    public class Order
    {
        public int Id { get; set; }

        public bool IsPaid { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }

        public string Waiter { get; set; }

        public string Submitter { get; set; }

        public double Total { get; set; }

        public int Table { get; set; }

        public DateTime SubmitDateTime { get; set; }

        public bool WaiterPayment { get; set; }
    }
}
