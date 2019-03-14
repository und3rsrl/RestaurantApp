using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RestaurantApp.Views.Administrator.Views.Helpers
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddCategoriePopupView : PopupPage
    {
        public AddCategoriePopupView()
        {
            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync(true);
        }
    }
}