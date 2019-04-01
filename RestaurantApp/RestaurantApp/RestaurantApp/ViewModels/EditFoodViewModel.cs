using Plugin.Media.Abstractions;
using RestaurantApp.Models;
using RestaurantApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace RestaurantApp.ViewModels
{
    public class EditFoodViewModel
    {
        private FoodApiService _foodsApiService = new FoodApiService();

        public int Id { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public string ImageUrl { get; set; }

        public string Category { get; set; }

        public string Ingredients { get; set; }

        public event EventHandler RefreshFoods;

        public MediaFile Image { get; set; }

        public ICommand EditFoodCommand
        {
            get
            {
                return new Command(async () =>
                {
                    FoodItem foodItem = new FoodItem()
                    {
                        Name = Name,
                        Price = Price,
                        ImageUrl = ImageUrl.Split('/').LastOrDefault(),
                        Category = Category,
                        Ingredients = Ingredients
                    };

                    var response = await _foodsApiService.EditFood(Id, foodItem, Image);

                    if (!string.IsNullOrEmpty(response))
                        await Application.Current.MainPage.DisplayAlert("Edit Categorie", response, "Ok");
                    else
                        RefreshFoods?.Invoke(this, EventArgs.Empty);
                });
            }
        }
    }
}
