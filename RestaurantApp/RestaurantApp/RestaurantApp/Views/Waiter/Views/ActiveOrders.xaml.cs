using RestaurantApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RestaurantApp.Views.Waiter.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ActiveOrders : ContentPage
	{
		public ActiveOrders ()
		{
			InitializeComponent ();
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var viewModel = BindingContext as WaiterActiveOrdersViewModel;

            viewModel.NoOrdersUIHandler += NoOrdersUI;
            viewModel.HasOrdersUIHandler += HasOrdersUI;
            viewModel.EndRefreshHandler += EndRefresh;

            if (viewModel != null)
            {
                if (viewModel.GetOrders.CanExecute(null))
                    viewModel.GetOrders.Execute(null);
            }            
        }

        private void HasOrdersUI(object sender, EventArgs e)
        {
            OrdersListView.IsVisible = true;
            NoActiveOrder_Layout.IsVisible = false;
        }

        private void EndRefresh(object sender, EventArgs e)
        {
            OrdersListView.EndRefresh();
        }

        private void NoOrdersUI(object sender, EventArgs e)
        {
            OrdersListView.IsVisible = false;
            NoActiveOrder_Layout.IsVisible = true;
        }
    }
}