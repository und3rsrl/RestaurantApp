using RestaurantApp.DTOs;
using RestaurantApp.Handlers;
using RestaurantApp.Helpers;
using RestaurantApp.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace RestaurantApp.ViewModels
{
    public class LoginViewModel
    {
        private ApiServices _apiServices = new ApiServices();

        public LoginViewModel()
        {

        }

        public LoginViewModel(INavigation navigation, Page page)
        {
            Navigation = navigation;
            Page = page;
        }

        public string Username { get; set; }

        public string Password { get; set; }

        public INavigation Navigation { get; set; }

        public Page Page { get; set; }

        public ICommand LoginCommand
        {
            get
            {
                return new Command(async() => 
                {
                    if (!CheckInformation())
                    {
                        await Page.DisplayAlert("Login", "Username or Password cannot be empty.", "Ok");
                    }
                    else
                    {
                        var token = await _apiServices.LoginAsync(Username, Password);

                        if (token.Equals("Unauthorized", StringComparison.OrdinalIgnoreCase))
                            await Page.DisplayAlert("Login Failed", "Username or Password are wrong.", "Ok");
                        else
                        {
                            var payload = JWT.JsonWebToken.DecodeToObject<JWTPayloadDTO>(token, "alexandruGeorgianChiurtu");

                            Settings.AccessToken = token;

                            var userWindow = UserWindowFactory.GenerateWindow(payload);
                            if (Device.RuntimePlatform == Device.Android)
                            {
                                Application.Current.MainPage = userWindow;
                            }
                            else if (Device.RuntimePlatform == Device.iOS)
                            {
                                await Navigation.PushModalAsync(userWindow);
                            }
                        }
                    }               
                });
            }
        }

        private bool CheckInformation()
        {
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
                return false;

            return true;
        }
    }
}
