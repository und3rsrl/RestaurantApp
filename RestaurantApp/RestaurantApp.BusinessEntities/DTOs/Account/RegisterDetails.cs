using System.ComponentModel.DataAnnotations;

namespace RestaurantApp.BusinessEntities.DTOs.Account
{
    public class RegisterDetails
    {
        [Required]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "PASSWORD_MIN_LENGTH", MinimumLength = 6)]
        public string Password { get; set; }
    }
}
