using MvvmHelpers;
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
    public class AddFoodViewModel : BaseViewModel
    {
        private CategoriesApiService _categoriesApiService = new CategoriesApiService();
        private FoodApiService _foodsApiService = new FoodApiService();
        private string _selectedCategorie;
        private List<CategorieItem> _categories;

        public AddFoodViewModel()
        {
            Categories = new ObservableRangeCollection<string>();
        }

        public AddFoodViewModel(Page page, List<CategorieItem> categories)
        {
            Categories = new ObservableRangeCollection<string>();
            Page = page;
            _categories = categories;
        }

        public ObservableRangeCollection<string> Categories
        {
            get; private set;
        }

        public string Name { get; set; }

        public string Ingredients { get; set; }

        public double Price { get; set; }

        public Page Page { get; set; }

        public string SelectedCategorie
        {
            get => _selectedCategorie;
            set
            {
                SetProperty(ref _selectedCategorie, value);
            }
        }

        public MediaFile Image { get; set; }

        public event EventHandler RefreshFoods;

        public event EventHandler SuccessfulAdd;

        public ICommand AddFoodCommand
        {
            get
            {
                return new Command(async () =>
                {
                    if (!Validate(Name, Price, SelectedCategorie, Image, out List<string> messages))
                    {
                        string description = FormatMessage(messages);

                        await Page.DisplayAlert("Foods", description, "Ok");
                    }
                    else
                    {
                        var response = await _foodsApiService.AddFood(Name, Ingredients, Price, _categories.FirstOrDefault(x => x.Name == SelectedCategorie).Id, Image);

                        if (!string.IsNullOrEmpty(response))
                            await Application.Current.MainPage.DisplayAlert("Add Categorie", response, "Ok");
                        else
                        {
                            RefreshFoods?.Invoke(this, EventArgs.Empty);
                            SuccessfulAdd?.Invoke(this, EventArgs.Empty);
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
