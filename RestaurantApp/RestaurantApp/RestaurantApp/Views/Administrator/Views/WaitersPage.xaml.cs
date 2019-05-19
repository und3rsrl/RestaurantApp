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
	public partial class WaitersPage : ContentPage
	{
		public WaitersPage ()
		{
			InitializeComponent ();
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var viewModel = BindingContext as WaitersViewModel;

            if (viewModel != null)
            {
                if (viewModel.LoadWaiters.CanExecute(null))
                    viewModel.LoadWaiters.Execute(null);
            }

            WaitersListView.RefreshCommand = viewModel.LoadWaiters;
            viewModel.EndRefreshHandler += EndRefresh;
        }

        private void EndRefresh(object sender, EventArgs e)
        {
            WaitersListView.EndRefresh();
        }

        private void Button_AddWaiter_Clicked(object sender, EventArgs e)
        {
            var viewModel = BindingContext as WaitersViewModel;

            PopupNavigation.Instance.PushAsync(new AddWaiterPopupView(viewModel.Refresh));
        }
    }
}