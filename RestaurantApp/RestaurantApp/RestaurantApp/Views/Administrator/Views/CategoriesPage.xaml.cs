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
	public partial class CategoriesPage : ContentPage
	{
		public CategoriesPage ()
		{
			InitializeComponent ();

            CategoriesListView.SeparatorVisibility = SeparatorVisibility.None;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var viewModel = BindingContext as CategoriesViewModel;

            if (viewModel != null)
            {
                if (viewModel.LoadCategories.CanExecute(null))
                    viewModel.LoadCategories.Execute(null);
            }

            CategoriesListView.RefreshCommand = viewModel.LoadCategories;
        }

        private void Button_AddCategorie_Clicked(object sender, EventArgs e)
        {
            var viewModel = BindingContext as CategoriesViewModel;

            PopupNavigation.Instance.PushAsync(new AddCategoriePopupView(viewModel.Refresh));
        }

        private async void CategoriesListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var viewModel = BindingContext as CategoriesViewModel;
            var selectedCategorie = e.SelectedItem as CategorieItem;
            await PopupNavigation.Instance.PushAsync(new EditCategoriePopupView(selectedCategorie, viewModel.Refresh));
        }
    }
}