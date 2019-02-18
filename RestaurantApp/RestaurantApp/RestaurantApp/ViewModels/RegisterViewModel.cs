using RestaurantApp.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace RestaurantApp.ViewModels
{
    public class RegisterViewModel
    {
        ApiServices _apiServices = new ApiServices();

        public string Email { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public string Message { get; set; }

        public ICommand RegisterCommand
        {
            get
            {
                return new Command(async() => {
                    var isSuccess = await _apiServices.RegisterAsync(Email, Password, ConfirmPassword);

                    if (isSuccess)
                        Message = "Register successfully";
                    else
                        Message = "Register failed.";
                });
            }
        }
    }
}
