using RestaurantApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RestaurantApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ForgotPasswordPage : ContentPage
	{
		public ForgotPasswordPage ()
		{
            InitializeComponent();
            BindingContext = new ForgotPasswordViewModel(this);		

            var viewModel = BindingContext as ForgotPasswordViewModel;

            viewModel.CodeWasSent += CodeWasSent;
            viewModel.CodeIsValid += CodeIsValid;
        }

        private void CodeWasSent(object sender, EventArgs e)
        {
            SendEmailButton.IsVisible = false;
            SendEmailEntry.IsVisible = false;
            SendCodeEntry.IsVisible = true;
            SendCodeButton.IsVisible = true;
        }

        private void CodeIsValid(object sender, EventArgs e)
        {
            SendCodeEntry.IsVisible = false;
            SendCodeButton.IsVisible = false;
            SendConfirmPasswordEntry.IsVisible = true;
            SendPasswordEntry.IsVisible = true;
            SendPasswordButton.IsVisible = true;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new WelcomePage ();
        }
    }
}