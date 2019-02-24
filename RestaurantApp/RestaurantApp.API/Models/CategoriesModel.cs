using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RestaurantApp.API.Models
{
    public class CategoriesModel
    {
        [Required]
        [Key]
        public int Identifier { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}