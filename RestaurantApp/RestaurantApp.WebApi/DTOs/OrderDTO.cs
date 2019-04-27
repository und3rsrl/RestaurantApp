using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantApp.WebApi.DTOs
{
    public class OrderDTO
    {
        public string Submitter { get; set; }

        public DateTime SubmiteDatetime { get; set; }

        public int Table { get; set; }

        public double Total { get; set; }

        public ICollection<OrderItemDTO> OrderItems { get; set; }
    }
}
