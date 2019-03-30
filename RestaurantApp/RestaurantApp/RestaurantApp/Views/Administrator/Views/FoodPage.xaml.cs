using RestaurantApp.Models;
using RestaurantApp.ViewModels;
using RestaurantApp.Views.Administrator.Views.Helpers;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RestaurantApp.Views.Administrator.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FoodPage : ContentPage
	{
		public FoodPage ()
		{
			InitializeComponent ();
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var viewModel = BindingContext as FoodItemsViewModel;

            if (viewModel != null)
            {
                if (viewModel.LoadFoods.CanExecute(null))
                    viewModel.LoadFoods.Execute(null);
            }

            FoodListView.RefreshCommand = viewModel.LoadFoods;
        }

        private void Button_AddFood_Clicked(object sender, EventArgs e)
        {
            //var viewModel = BindingContext as CategoriesViewModel;

            PopupNavigation.Instance.PushAsync(new AddFoodPopupView());
        }

        private async void FoodListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var viewModel = BindingContext as FoodItemsViewModel;
            var slectedFoodItem = e.SelectedItem as FoodItem;
            //await PopupNavigation.Instance.PushAsync();
        }
    }
}