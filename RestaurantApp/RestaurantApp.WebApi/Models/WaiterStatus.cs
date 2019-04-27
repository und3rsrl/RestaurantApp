using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantApp.WebApi.Models
{
    public class WaiterStatus
    {
        [Key]
        public string Waiter { get; set; }

        public bool IsActive { get; set; }
    }
}
