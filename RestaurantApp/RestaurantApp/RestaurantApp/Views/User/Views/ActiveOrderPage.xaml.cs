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
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var viewModel = BindingContext as ActiveOrderViewModel;

            viewModel.NoActiveOrderUIHandler += NoActiveOrderUI;
            viewModel.HasActiveOrderUIHandler += HasActiveOrderUI;

            if (viewModel != null)
            {
                if (viewModel.GetActiveOrder.CanExecute(null))
                    viewModel.GetActiveOrder.Execute(null);
            }
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
    }
}