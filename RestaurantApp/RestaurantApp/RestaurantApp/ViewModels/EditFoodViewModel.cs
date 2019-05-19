using Plugin.Media.Abstractions;
using Plugin.Toasts;
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

        public EditFoodViewModel()
        {

        }

        public EditFoodViewModel(Page page)
        {
            Page = page;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public string ImageUrl { get; set; }

        public string Category { get; set; }

        public string Ingredients { get; set; }

        public event EventHandler RefreshFoods;

        public event EventHandler SuccessfulEdit;

        public MediaFile Image { get; set; }

        public Page Page { get; set; }

        public ICommand EditFoodCommand
        {
            get
            {
                return new Command(async () =>
                {
                    if (!Validate(Name, Price, Category, Image, out List<string> messages))
                    {
                        string description = FormatMessage(messages);

                        await Page.DisplayAlert("Foods", description, "Ok");
                    }
                    else
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
                        {
                            RefreshFoods?.Invoke(this, EventArgs.Empty);
                            SuccessfulEdit?.Invoke(this, EventArgs.Empty);
                        }
                    }
                });
            }
        }

        private bool Validate(string name, double price, string selectedCategorie, MediaFile image, out List<string> messages)
        {
            bool isValid = true;
            messages = new List<string>();

            if (string.IsNullOrEmpty(name))
            {
                messages.Add("Please provide a Food Name!");
                isValid = false;
            }

            if (price <= 0d)
            {
                messages.Add("Please provide the price!");
                isValid = false;
            }

            if (string.IsNullOrEmpty(selectedCategorie))
            {
                messages.Add("Please select the category!");
                isValid = false;
            }

            if (image == null)
            {
                messages.Add("Please provide an image!");
                isValid = false;
            }

            return isValid;
        }

        private string FormatMessage(List<string> messages)
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (var message in messages)
            {
                stringBuilder.AppendLine(message);
            }

            return stringBuilder.ToString();
        }
    }
}
