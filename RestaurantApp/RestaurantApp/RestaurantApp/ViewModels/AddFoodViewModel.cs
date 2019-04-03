using MvvmHelpers;
using Plugin.Media.Abstractions;
using RestaurantApp.Models;
using RestaurantApp.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace RestaurantApp.ViewModels
{
    public class AddFoodViewModel : BaseViewModel
    {
        private CategoriesApiService _categoriesApiService = new CategoriesApiService();
        private FoodApiService _foodsApiService = new FoodApiService();
        private string _selectedCategorie;

        public AddFoodViewModel()
        {
            Categories = new ObservableRangeCollection<string>();
        }

        public ObservableRangeCollection<string> Categories
        {
            get; private set;
        }

        public string Name { get; set; }

        public string Ingredients { get; set; }

        public double Price { get; set; }

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

        public ICommand AddFoodCommand
        {
            get
            {
                return new Command(async () =>
                {
                    var response = await _foodsApiService.AddFood(Name, Ingredients, Price, SelectedCategorie, Image);

                    if (!string.IsNullOrEmpty(response))
                        await Application.Current.MainPage.DisplayAlert("Add Categorie", response, "Ok");
                    else
                        RefreshFoods?.Invoke(this, EventArgs.Empty);
                });
            }
        }
    }
}
