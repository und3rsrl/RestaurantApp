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
	public partial class AddWaiterPopupView : PopupPage
	{
		public AddWaiterPopupView (EventHandler refreshWaiters)
		{
			InitializeComponent ();
		}

        private void BindData(EventHandler refreshWaiters)
        {
            var viewModel = BindingContext as AddWaiterViewModel;
            viewModel.RefreshWaiters += refreshWaiters;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync(true);
        }
    }
}