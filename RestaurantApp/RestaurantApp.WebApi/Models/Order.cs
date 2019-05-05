using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantApp.WebApi.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

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
