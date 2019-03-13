using RestaurantApp.Models;
using RestaurantApp.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantApp.ViewModels
{
    public class FoodItemsViewModel : INotifyPropertyChanged
    {
        private string _selectedCategorie;
        private List<string> _categories;
        private CategoriesApiService _categoriesApiService = new CategoriesApiService();

        public FoodItemsViewModel()
        {
            _selectedCategorie = "All";

            AllItems =  new List<FoodItem>()
            {
                new FoodItem()
                {
                    Id = 1,
                    Name = "Salata de rosii",
                    Price = 10.99,
                    Category = "Salads",
                    Ingredients = new List<string>()
                    {
                        "Rosii",
                        "Castraveti"
                    },
                    ImageUrl = "https://images.pexels.com/photos/1211887/pexels-photo-1211887.jpeg?auto=compress&cs=tinysrgb&dpr=2&h=750&w=1260"
                },
                new FoodItem()
                {
                    Id = 1,
                    Name = "Salata de castravet",
                    Price = 13.99,
                    Category = "Salads",
                    Ingredients = new List<string>()
                    {
                        "Castraveti"
                    },
                     ImageUrl = "https://images.pexels.com/photos/1435893/pexels-photo-1435893.jpeg?auto=compress&cs=tinysrgb&dpr=2&h=750&w=1260"
                },
                new FoodItem()
                {
                    Id = 1,
                    Name = "Pizza Quatro Stagioni",
                    Price = 21.99,
                    Category = "Pizza",
                    Ingredients = new List<string>()
                    {
                        "Masline"
                    },
                    ImageUrl = "https://images.pexels.com/photos/724216/pexels-photo-724216.jpeg?auto=compress&cs=tinysrgb&dpr=2&h=750&w=1260"
                },
                new FoodItem()
                {
                    Id = 1,
                    Name = "Pizza Diavola",
                    Price = 25.99,
                    Category = "Pizza",
                    Ingredients = new List<string>()
                    {
                        "Picant"
                    },
                    ImageUrl = "https://images.pexels.com/photos/532779/pexels-photo-532779.jpeg?auto=compress&cs=tinysrgb&dpr=2&h=750&w=1260"
                },
            };

            //_categories = LoadCategories().Result;
            FilteredItems = AllItems;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public List<FoodItem> AllItems { get; set; }

        public List<FoodItem> FilteredItems { get; set; }

        public List<string> Categories
        {
            get
            {
                return LoadCategories().Result;
            }
        }

        public string SelectedCategory
        {
            get => _selectedCategorie;
            set
            {
                if (_selectedCategorie != value)
                {
                    _selectedCategorie = value;
                    FilterItems();
                    OnPropertyChanged("FilteredItems");
                }                    
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void FilterItems()
        {
            if (_selectedCategorie.Equals("All", StringComparison.OrdinalIgnoreCase))
                FilteredItems = AllItems;
            else
                FilteredItems = AllItems.Where(x => x.Category.Equals(SelectedCategory, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        private async Task<List<string>> LoadCategories()
        {
            var categoriesItems = await _categoriesApiService.GetCategories().ConfigureAwait(false);

            return categoriesItems.Select(x => x.Name).ToList();
        }
    }
}
