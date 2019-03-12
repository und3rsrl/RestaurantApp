using JWT.exceptions;
using RestaurantApp.DTOs;
using RestaurantApp.Handlers;
using RestaurantApp.Helpers;
using RestaurantApp.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace RestaurantApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            SetMainPage();
        }

        private void SetMainPage()
        {
            if (!string.IsNullOrEmpty(Settings.AccessToken))
            {
                try
                {
                    var payload = JWT.JsonWebToken.DecodeToObject<JWTPayloadDTO>(Settings.AccessToken, "alexandruGeorgianChiurtu");

                    MainPage = UserWindowFactory.GenerateWindow(payload);
                }
                catch (SignatureVerificationException e)
                {
                    if (e.Message.Equals("Token has expired."))
                    {
                        MainPage = new WelcomePage();
                    }
                    else
                        throw e;
                }


            }
            else
                MainPage = new WelcomePage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
