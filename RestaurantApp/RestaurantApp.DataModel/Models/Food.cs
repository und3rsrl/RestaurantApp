using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantApp.WebApi.Models
{
    public class Food
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Ingredients { get; set; }

        public double Price { get; set; }

        public string Category { get; set; }

        public string ImageUrl { get; set; }
    }
}
