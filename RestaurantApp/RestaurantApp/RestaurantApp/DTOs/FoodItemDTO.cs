using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantApp.DTOs
{
    public class FoodItemDTO
    {
        public string Name { get; set; }

        public double Price { get; set; }

        public string Ingredients { get; set; }

        public string Categorie { get; set; }
    }
}
