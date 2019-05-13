using MvvmHelpers;
using Plugin.LocalNotifications;
using RestaurantApp.Helpers;
using RestaurantApp.Models;
using RestaurantApp.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Plugin.Toasts;

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

        public ICommand DeleteFoodCommand
        {
            get
            {
                return new Command<int>(async (id) =>
                {
                    await _foodsApiService.DeleteFood(id);
                    Refresh(this, EventArgs.Empty);
                });
            }
        }

        public ICommand AddToCart
        {
            get
            {
                return new Command<int>(async (productId) =>
                {
                    OrderItem orderItem = new OrderItem()
                    {
                        ProductId = productId,
                        Name = AllItems.Where(x => x.Id == productId).FirstOrDefault().Name,
                        Amount = 1,
                        Price = AllItems.Where(x => x.Id == productId).FirstOrDefault().Price,
                    };

                    orderItem.Total = orderItem.Amount * orderItem.Price;
                    var notificator = DependencyService.Get<IToastNotificator>();

                    var options = new NotificationOptions()
                    {
                        Title = "Cart",
                        ClearFromHistory = true,
                        Description = "You've added food to cart!",
                        IsClickable = false // Set to true if you want the result Clicked to come back (if the user clicks it)
                    };

                    Settings.Bascket.AddOrderItem(orderItem);

                    var result = await notificator.Notify(options);                    
                });
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

        public async void Refresh(object sender, EventArgs e)
        {
            await ExecuteLoadFoodsCommand();
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
                    food.ImageUrl = ApiService.BASE_SERVER_IMAGE_URL + food.ImageUrl;
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
