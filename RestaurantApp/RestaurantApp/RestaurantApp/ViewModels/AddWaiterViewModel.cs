using MvvmHelpers;
using RestaurantApp.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Xamarin.Forms;

namespace RestaurantApp.ViewModels
{
    public class AddWaiterViewModel : BaseViewModel
    {
        private WaitersApiService _waitersApiService = new WaitersApiService();

        public AddWaiterViewModel()
        {

        }

        public AddWaiterViewModel(Page page)
        {
            Page = page;
        }

        public string Email { get; set; }

        public string Password { get; set; }

        public Page Page { get; set; }

        public event EventHandler RefreshWaiters;

        public event EventHandler SuccessfulAdd;

        public ICommand AddWaiterCommand
        {
            get
            {
                return new Command(async () =>
                {
                    if (!CheckEmailIsValid())
                    {
                        await Page.DisplayAlert("Waiters", "Email is not valid.", "Ok");
                    }
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

                            await Page.DisplayAlert("Waiters", passwordNotValidMessage.ToString(), "Ok");
                        }
                        else
                        {
                            var response = await _waitersApiService.AddWaiter(Email, Password);

                            if (!string.IsNullOrEmpty(response))
                                await Application.Current.MainPage.DisplayAlert("Add Categorie", response, "Ok");
                            else
                            {
                                RefreshWaiters?.Invoke(this, EventArgs.Empty);
                                SuccessfulAdd?.Invoke(this, EventArgs.Empty);       
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
