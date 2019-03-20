using MvvmHelpers;
using RestaurantApp.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace RestaurantApp.ViewModels
{
    public class AddWaiterViewModel : BaseViewModel
    {
        private WaitersApiService _waitersApiService = new WaitersApiService();

        public string Email { get; set; }

        public string Password { get; set; }

        public event EventHandler RefreshWaiters;

        public ICommand AddWaiterCommand
        {
            get
            {
                return new Command(async () =>
                {
                    var response = await _waitersApiService.AddWaiter(Email, Password);

                    if (!string.IsNullOrEmpty(response))
                        await Application.Current.MainPage.DisplayAlert("Add Categorie", response, "Ok");
                    else
                        RefreshWaiters?.Invoke(this, EventArgs.Empty);
                });
            }
        }
    }
}
