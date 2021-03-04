using System.ComponentModel.DataAnnotations;

namespace RestaurantApp.DataModel.Models
{
    public class WaiterStatus
    {
        [Key]
        public string Waiter { get; set; }

        public bool IsActive { get; set; }
    }
}
