using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantApp.Models
{
    public class Order
    {        
        public string Submitter { get; set; }

        public DateTime SubmitDatetime { get; set; }

        public int Table { get; set; }

        public double Total { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
