using RestaurantApp.DTOs;
using RestaurantApp.Handlers;
using RestaurantApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Xamarin.Forms;

namespace RestaurantApp.ViewModels
{
    public class RegisterViewModel
    {
        ApiServices _apiServices = new ApiServices();

        public RegisterViewModel()
        {

        }

        public RegisterViewModel(INavigation navigation, Page page)
        {
            Navigation = navigation;
            Page = page;
        }

        public string Email { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public string Message { get; set; }

        public INavigation Navigation { get; set; }

        public Page Page { get; set; }

        public ICommand RegisterCommand
        {
            get
            {
                return new Command(async() => {

                    if (!CheckEmailIsValid())
                        await Page.DisplayAlert("Register", "Email is not valid.", "Ok");
                    else
                    {
                        if (!CheckPasswordIsValid())
                        {
                            StringBuilder passwordNotValidMessage = new StringBuilder();
                            passwordNotValidMessage.AppendLine("Password is not valid.");
                            passwordNotValidMessage.AppendLine("It must contain:");
                            passwordNotValidMessage.AppendLine("- Minimum 6 characters");
                            passwordNotValidMessage.AppendLine("- At least one uppercase letter");
                            passwordNotValidMessage.AppendLine("- At least one lowercase letter");
                            passwordNotValidMessage.AppendLine("- At least one number");

                            await Page.DisplayAlert("Register", passwordNotValidMessage.ToString(), "Ok");
                        }
                        else
                        {
                            if (!Password.Equals(ConfirmPassword, StringComparison.OrdinalIgnoreCase))
                                await Page.DisplayAlert("Register", "Password doesn't match with the Confirm Password.", "Ok");
                            else
                            {
                                var isSuccess = await _apiServices.RegisterAsync(Email, Password, ConfirmPassword);

                                if (isSuccess.Equals("NotCreated", StringComparison.OrdinalIgnoreCase))
                                    await Page.DisplayAlert("Register", "Something went wrong. Try again.", "Ok");
                                else
                                {
                                    var payload = JWT.JsonWebToken.DecodeToObject<JWTPayloadDTO>(isSuccess, "alexandruGeorgianChiurtu");

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
                        }
                    }
                });
            }
        }

        private bool CheckEmailIsValid()
        {
            if (string.IsNullOrEmpty(Email))
                return false;

            var regex = new Regex(@"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$");

            if (!regex.IsMatch(Email))
                return false;

            return true;
        }

        private bool CheckPasswordIsValid()
        {
            if (string.IsNullOrEmpty(Password))
                return false;

            var regex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{6,}$");

            if (!regex.IsMatch(Password))
                return false;

            return true;
        }
    }
}
