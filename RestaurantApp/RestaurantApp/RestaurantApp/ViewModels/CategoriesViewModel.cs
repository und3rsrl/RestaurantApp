using MvvmHelpers;
using RestaurantApp.Models;
using RestaurantApp.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace RestaurantApp.ViewModels
{
    public class CategoriesViewModel : BaseViewModel
    {
        private CategoriesApiService _categoriesApiService = new CategoriesApiService();

        public CategoriesViewModel()
        {
            Categories = new ObservableRangeCollection<CategorieItem>();
        }
    
        public ObservableRangeCollection<CategorieItem> Categories
        {
            get; private set;
        }

        public async void Refresh(object sender, EventArgs e)
        {
            await ExecuteLoadCategoriesCommand();
        }

        public ICommand LoadCategories
        {
            get
            {
                return new Command(async () => await ExecuteLoadCategoriesCommand());
            }
        }

        public ICommand DeleteCategorieCommand
        {
            get
            {
                return new Command<int>(async (id) =>
                {
                    await _categoriesApiService.DeleteCategorie(id);
                    Refresh(this, EventArgs.Empty);
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
                Categories.ReplaceRange(items);
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
