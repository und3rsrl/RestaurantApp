using System.ComponentModel.DataAnnotations;

namespace RestaurantApp.BusinessEntities.DTOs.Account
{
    public class LoginDetails
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
