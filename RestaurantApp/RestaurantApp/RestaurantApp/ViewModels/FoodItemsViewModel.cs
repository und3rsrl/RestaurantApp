using MvvmHelpers;
using RestaurantApp.Models;
using RestaurantApp.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace RestaurantApp.ViewModels
{
    public class FoodItemsViewModel : BaseViewModel
    {
        private string _selectedCategorie;
        private List<string> _categories;
        private CategoriesApiService _categoriesApiService = new CategoriesApiService();
        private FoodApiService _foodsApiService = new FoodApiService();

        public FoodItemsViewModel()
        {
            _selectedCategorie = "All";
            FilteredItems = new ObservableRangeCollection<FoodItem>();
            AllItems = new ObservableRangeCollection<FoodItem>();
        }

        public ObservableRangeCollection<FoodItem> AllItems { get; set; }

        public ObservableRangeCollection<FoodItem> FilteredItems { get; set; }

        public ObservableRangeCollection<string> Categories
        {
            get
            {
                var categories = new ObservableRangeCollection<string>();

                categories.AddRange(LoadCategories().Result);

                return categories;
            }
        }

        public ICommand LoadFoods
        {
            get
            {
                return new Command(async () => await ExecuteLoadFoodsCommand());
            }
        }

        public string SelectedCategory
        {
            get => _selectedCategorie;
            set
            {
                if (SetProperty(ref _selectedCategorie, value))
                    FilterItems();
            }
        }

        private void FilterItems()
        {
            FilteredItems.ReplaceRange(AllItems.Where(x => x.Category == SelectedCategory || SelectedCategory == "All"));
        }

        private async Task<List<string>> LoadCategories()
        {
            var categoriesItems = await _categoriesApiService.GetCategories().ConfigureAwait(false);

            return categoriesItems.Select(x => x.Name).ToList();
        }

        private async Task ExecuteLoadFoodsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                var items = await _foodsApiService.GetFoods();

                foreach (var food in items)
                {
                    food.ImageUrl = ApiService.BASE_SERVER_URL + food.ImageUrl;
                }

                AllItems.ReplaceRange(items);
                FilterItems();
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
