using RestaurantApp.Models;
using RestaurantApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RestaurantApp.Views.User.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BasketPage : ContentPage
	{
		public BasketPage ()
		{
			InitializeComponent ();
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var viewModel = BindingContext as BasketViewModel;

            var total = 0d;

            foreach (OrderItem item in viewModel.FoodItems)
            {
                total += item.Total;
            }

            Total_Label.Text = string.Format("Total: {0} lei", total);
        }
    }
}