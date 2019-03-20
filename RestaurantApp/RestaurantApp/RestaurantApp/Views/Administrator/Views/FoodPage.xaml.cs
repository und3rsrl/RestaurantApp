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

        private void Button_AddFood_Clicked(object sender, EventArgs e)
        {
            //var viewModel = BindingContext as CategoriesViewModel;

            PopupNavigation.Instance.PushAsync(new AddFoodPopupView());
        }
    }
}