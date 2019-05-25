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
	public partial class HistoryOrdersPage : ContentPage
	{
		public HistoryOrdersPage ()
		{
			InitializeComponent ();
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var viewModel = BindingContext as HistoryOrdersViewModel;

            viewModel.NoOrdersUIHandler += NoActiveOrderUI;
            viewModel.HasOrdersUIHandler += HasActiveOrderUI;

            if (viewModel != null)
            {
                if (viewModel.GetPreviousOrders.CanExecute(null))
                    viewModel.GetPreviousOrders.Execute(null);
            }
        }

        private void HasActiveOrderUI(object sender, EventArgs e)
        {
            HistoryOrdersListView.IsVisible = true;
            NoActiveOrder_Layout.IsVisible = false;
        }

        private void NoActiveOrderUI(object sender, EventArgs e)
        {
            HistoryOrdersListView.IsVisible = false;
            NoActiveOrder_Layout.IsVisible = true;
        }

        private void HistoryOrdersListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }
    }
}