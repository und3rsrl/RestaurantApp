using MvvmHelpers;
using RestaurantApp.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace RestaurantApp.ViewModels
{
    public class EditCategorieViewModel : BaseViewModel
    {
        private CategoriesApiService _categoriesApiService = new CategoriesApiService();

        public int Id { get; set; }

        public string Name { get; set; }

        public event EventHandler RefreshCategories;

        public ICommand EditCategorieCommand
        {
            get
            {
                return new Command(async () =>
                {
                    var response = await _categoriesApiService.EditCategorie(Id, Name);

                    if (!string.IsNullOrEmpty(response))
                        await Application.Current.MainPage.DisplayAlert("Edit Categorie", response, "Ok");
                    else
                        RefreshCategories?.Invoke(this, EventArgs.Empty);                    
                });
            }
        }
    }
}
