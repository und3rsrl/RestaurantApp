using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantApp.WebApi.DTOs
{
    public class FoodDTO
    {
        public string Name { get; set; }

        public double Price { get; set; }

        public string Ingredients { get; set; }

        public string Categorie { get; set; }
    }
}
