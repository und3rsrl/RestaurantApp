using MvvmHelpers;
using RestaurantApp.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace RestaurantApp.ViewModels
{
    public class AddCategorieViewModel : BaseViewModel
    {
        private CategoriesApiService _categoriesApiService = new CategoriesApiService();

        public string Name { get; set; }

        public event EventHandler RefreshCategories;

        public ICommand AddCategorieCommand
        {
            get
            {
                return new Command(async () =>
                {
                    var response = await _categoriesApiService.AddCategorie(Name);

                    if (!string.IsNullOrEmpty(response))
                        await Application.Current.MainPage.DisplayAlert("Add Categorie", response, "Ok");
                    else
                        RefreshCategories?.Invoke(this, EventArgs.Empty);
                });
            }
        }
    }
}
