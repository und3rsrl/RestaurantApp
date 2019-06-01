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
            BindingContext = new BasketViewModel(this);
            ((BasketViewModel)BindingContext).ResetTotal += ResetTotal;
            ((BasketViewModel)BindingContext).RecalculateTotal += RecalculateTotal;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            CalculateTotal();
        }

        private void RecalculateTotal(object sender, EventArgs e)
        {
            CalculateTotal();
        }

        private void ResetTotal(object sender, EventArgs e)
        {
            var viewModel = BindingContext as BasketViewModel;
            viewModel.Total = 0;
            Total_Label.Text = string.Format("Total: {0} lei", 0);
            Table_Picker.SelectedItem = null;
        }

        private void CalculateTotal()
        {
            var viewModel = BindingContext as BasketViewModel;
            var total = 0d;

            foreach (OrderItem item in viewModel.FoodItems)
            {
                total += item.Total;
            }

            viewModel.Total = total;

            Total_Label.Text = string.Format("Total: {0} lei", total);
        }
    }
}