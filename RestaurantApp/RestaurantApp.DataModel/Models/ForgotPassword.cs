using System;
using System.ComponentModel.DataAnnotations;

namespace RestaurantApp.DataModel.Models
{
    public class ForgotPassword
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public string CreatedBy { get; set; }

        [Required]
        public DateTime LastUpdatedAt { get; set; }

        [Required]
        public string LastUpdatedBy { get; set; }
    }
}
