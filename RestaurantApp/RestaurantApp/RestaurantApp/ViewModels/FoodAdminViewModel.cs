using RestaurantApp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantApp.ViewModels
{
    public class FoodAdminViewModel
    {
        public List<FoodItem> Foods { get; set; }

        public FoodAdminViewModel()
        {
            Foods = new List<FoodItem>()
            {
                new FoodItem()
                {
                    Id = 1,
                    Name = "Salata de rosii",
                    Price = 10.99,
                    Category = "Salads",
                    //Ingredients = new List<string>()
                    //{
                    //    "Rosii",
                    //    "Castraveti"
                    //},
                    ImageUrl = "https://images.pexels.com/photos/1211887/pexels-photo-1211887.jpeg?auto=compress&cs=tinysrgb&dpr=2&h=750&w=1260"
                },
                new FoodItem()
                {
                    Id = 1,
                    Name = "Salata de castravet",
                    Price = 13.99,
                    Category = "Salads",
                    //Ingredients = new List<string>()
                    //{
                    //    "Castraveti"
                    //},
                     ImageUrl = "https://images.pexels.com/photos/1435893/pexels-photo-1435893.jpeg?auto=compress&cs=tinysrgb&dpr=2&h=750&w=1260"
                },
                new FoodItem()
                {
                    Id = 1,
                    Name = "Pizza Quatro Stagioni",
                    Price = 21.99,
                    Category = "Pizza",
                    //Ingredients = new List<string>()
                    //{
                    //    "Masline"
                    //},
                    ImageUrl = "https://images.pexels.com/photos/724216/pexels-photo-724216.jpeg?auto=compress&cs=tinysrgb&dpr=2&h=750&w=1260"
                },
                new FoodItem()
                {
                    Id = 1,
                    Name = "Pizza Diavola",
                    Price = 25.99,
                    Category = "Pizza",
                    //Ingredients = new List<string>()
                    //{
                    //    "Picant"
                    //},
                    ImageUrl = "https://images.pexels.com/photos/532779/pexels-photo-532779.jpeg?auto=compress&cs=tinysrgb&dpr=2&h=750&w=1260"
                },
            };
        }
    }
}
