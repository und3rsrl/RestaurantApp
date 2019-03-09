using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantApp.Models
{
    public class FoodItem
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public string ImageUrl { get; set; }

        public string Category { get; set; }

        public List<string> Ingredients { get; set; }
    }
}
