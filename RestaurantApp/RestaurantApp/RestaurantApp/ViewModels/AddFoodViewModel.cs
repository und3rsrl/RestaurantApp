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

        public string SelectedCategorie { get; set; }

        public MediaFile Image { get; set; }

        public event EventHandler RefreshFoods;

        public ICommand LoadCategories
        {
            get
            {
                return new Command(async () => await ExecuteLoadCategoriesCommand());
            }
        }

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

        private async Task ExecuteLoadCategoriesCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                var items = await _categoriesApiService.GetCategories();
                Categories.ReplaceRange(items.Select(x => x.Name).ToList());
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
