using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantApp.Models
{
    public class PreviousOrder
    {
        public DateTime SubmitDate { get; set; }

        public int Table { get; set; }

        public double Total { get; set; }
    }
}
