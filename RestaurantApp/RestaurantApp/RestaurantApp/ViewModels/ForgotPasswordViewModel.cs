using Plugin.Toasts;
using RestaurantApp.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Xamarin.Forms;

namespace RestaurantApp.ViewModels
{
    public class ForgotPasswordViewModel
    {
        private AuthApiServices service = new AuthApiServices();

        public ForgotPasswordViewModel(Page page)
        {
            Page = page;
        }

        public ForgotPasswordViewModel()
        {
        }

        public string Email { get; set; }

        public string VerificationCode { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public EventHandler CodeWasSent;

        public EventHandler CodeIsValid;

        public Page Page { get; set; }

        public ICommand SendEmailCommand
        {
            get
            {
                return new Command(async () =>
                {
                    var notificator = DependencyService.Get<IToastNotificator>();                  

                    if (CheckEmailIsValid())
                    {
                        var isCodeNotSent = !(await service.RequestPasswordChange(Email));

                        if (isCodeNotSent)
                        {
                            var options = new NotificationOptions()
                            {
                                Title = "Forgot Password",
                                ClearFromHistory = true,
                                Description = "Invalid email!",
                                IsClickable = false // Set to true if you want the result Clicked to come back (if the user clicks it)
                            };

                            var result = await notificator.Notify(options);
                        }
                        else
                        {
                            var options = new NotificationOptions()
                            {
                                Title = "Forgot Password",
                                ClearFromHistory = true,
                                Description = "Check your email for code!",
                                IsClickable = false // Set to true if you want the result Clicked to come back (if the user clicks it)
                            };
                            CodeWasSent?.Invoke(this, EventArgs.Empty);
                            var result = await notificator.Notify(options);
                        }                        
                    }
                    else
                    {
                        
                    }
                });
            }
        }

        public ICommand SendCodeCommand
        {
            get
            {
                return new Command(async () =>
                {
                    var notificator = DependencyService.Get<IToastNotificator>();

                    if (CheckEmailIsValid())
                    {
                        var isCodeNotValid = !(await service.SendCodeForValidation(Email, VerificationCode));

                        if (isCodeNotValid)
                        {
                            var options = new NotificationOptions()
                            {
                                Title = "Forgot Password",
                                ClearFromHistory = true,
                                Description = "Invalid code!",
                                IsClickable = false // Set to true if you want the result Clicked to come back (if the user clicks it)
                            };
                    
                            var result = await notificator.Notify(options);
                        }
                        else
                        {
                            CodeIsValid?.Invoke(this, EventArgs.Empty);
                        }
                    }
                    else
                    {

                    }
                });
            }
        }

        public ICommand ChangePasswordCommand
        {
            get
            {
                return new Command(async () => {

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
                                var isSuccess = await service.UpdatePassword(Email, Password);

                                if (!isSuccess)
                                {
                                    await Page.DisplayAlert("Forgot Password", "Something went wrong. Try again.", "Ok");
                                }
                                else
                                {
                                    await Page.DisplayAlert("Forgot Password", "Password changed with success", "Ok");
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
