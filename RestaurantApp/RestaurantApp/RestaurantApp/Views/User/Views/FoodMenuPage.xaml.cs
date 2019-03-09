using RestaurantApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RestaurantApp.Views.User.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FoodMenuPage : ContentPage
	{
		public FoodMenuPage ()
		{
			InitializeComponent ();
        
            FoodListView.SeparatorVisibility = SeparatorVisibility.None;
        }    

        private async void FoodListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var selectedPie = e.SelectedItem as FoodItem;
            await Navigation.PushAsync(new FoodDetailPage(selectedPie));
        }
    }
}