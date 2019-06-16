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
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            Setup();
            BindingContext = new LoginViewModel(Navigation, this);
        }

        private void Setup()
        {
            Entry_UserName.Completed += (s, e) => Entry_Password.Focus();
            Entry_Password.Completed += (s, e) => Button_LogIn.Focus();
        }

        private void Button_ForgotPassword_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new  ForgotPasswordPage();
        }
    }
}