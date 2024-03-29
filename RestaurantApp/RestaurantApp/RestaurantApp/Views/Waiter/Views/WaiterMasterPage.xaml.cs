﻿using RestaurantApp.Helpers;
using RestaurantApp.Services;
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
	public partial class WaiterMasterPage : ContentPage
	{
        private AuthApiServices _authApiServices = new AuthApiServices();

		public WaiterMasterPage ()
		{
			InitializeComponent ();
		}

        private async void Button_Logout_Clicked(object sender, EventArgs e)
        {
            Settings.AccessToken = string.Empty;

            //await _authApiServices.Logout(Settings.UserName);

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