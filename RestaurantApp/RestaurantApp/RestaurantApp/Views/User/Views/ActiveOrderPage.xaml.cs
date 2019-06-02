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
	public partial class ActiveOrderPage : ContentPage
	{
		public ActiveOrderPage ()
		{
			InitializeComponent ();
            BindingContext = new ActiveOrderViewModel(this);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var viewModel = BindingContext as ActiveOrderViewModel;

            viewModel.NoActiveOrderUIHandler += NoActiveOrderUI;
            viewModel.HasActiveOrderUIHandler += HasActiveOrderUI;
            viewModel.CalculateTotal += RecalculateTotal;

            if (viewModel != null)
            {
                if (viewModel.GetActiveOrder.CanExecute(null))
                    viewModel.GetActiveOrder.Execute(null);
            }
        }

        private void RecalculateTotal(object sender, EventArgs e)
        {
            CalculateTotal();
        }

        private void HasActiveOrderUI(object sender, EventArgs e)
        {
            ActiveOrderListView.IsVisible = true;
            NoActiveOrder_Layout.IsVisible = false;
        }

        private void NoActiveOrderUI(object sender, EventArgs e)
        {
            ActiveOrderListView.IsVisible = false;
            NoActiveOrder_Layout.IsVisible = true;
        }

        private void CalculateTotal()
        {
            var viewModel = BindingContext as ActiveOrderViewModel;
            var total = 0d;

            foreach (var item in viewModel.OrderItems)
            {
                total += item.Total;
            }

            Total_Label.Text = string.Format("Total: {0} lei", total);
        }
    }
}