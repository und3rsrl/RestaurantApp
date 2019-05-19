using RestaurantApp.Models;
using RestaurantApp.ViewModels;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RestaurantApp.Views.Administrator.Views.Helpers
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EditCategoriePopupView : PopupPage
    {
        public EditCategoriePopupView (CategorieItem categorieItem, EventHandler refreshCategories)
		{
			InitializeComponent ();
            BindingContext = new EditCategorieViewModel();
            BindData(categorieItem, refreshCategories);
        }

        private void BindData(CategorieItem item, EventHandler refreshCategories)
        {
            Name_Entry.Text = item.Name;
            var viewModel = BindingContext as EditCategorieViewModel;
            viewModel.Id = item.Id;
            viewModel.RefreshCategories += refreshCategories;
            viewModel.SuccesfulEdit += OnSuccesffulEdit;
        }

        private void OnSuccesffulEdit(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync(true);
        }
    }
}