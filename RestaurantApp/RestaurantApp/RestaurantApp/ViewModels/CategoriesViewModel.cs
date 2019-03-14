using RestaurantApp.Models;
using RestaurantApp.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace RestaurantApp.ViewModels
{
    public class CategoriesViewModel : INotifyPropertyChanged
    {
        private CategoriesApiService _categoriesApiService = new CategoriesApiService();
        private IEnumerable<CategorieItem> _categorieItems;

        public CategoriesViewModel()
        {
            _categorieItems = _categoriesApiService.GetCategories().Result;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public int Id { get; set; }

        public string Name { get; set; }

        public Page Page { get; set; }

        public ICommand AddCategorieCommand
        {
            get
            {
                return new Command(async () =>
                {
                    var response = await _categoriesApiService.AddCategorie(Name);

                    if (!string.IsNullOrEmpty(response))
                        await Application.Current.MainPage.DisplayAlert("Add Categorie", response, "Ok");

                    Categories = _categoriesApiService.GetCategories().Result;
                });
            }
        }

        public IEnumerable<CategorieItem> Categories
        {
            get
            {
                return _categorieItems;
            }

            private set
            {
                _categorieItems = value;
                OnPropertyChanged(nameof(Categories));
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            Application.Current.MainPage.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
