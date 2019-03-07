using RestaurantApp.Helpers;
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
	public partial class UserMasterPage : ContentPage
	{
		public UserMasterPage ()
		{
			InitializeComponent ();
		}

        private async void Button_Logout_Clicked(object sender, EventArgs e)
        {
            Settings.AccessToken = string.Empty;

            if (Device.RuntimePlatform == Device.Android)
            {
                Application.Current.MainPage = new WelcomePage();
            }
            else if (Device.RuntimePlatform == Device.iOS)
            {
                await Navigation.PushModalAsync(new WelcomePage());
            }
        }
    }
}