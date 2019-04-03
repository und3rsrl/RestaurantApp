using RestaurantApp.Models;
using RestaurantApp.ViewModels;
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

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var viewModel = BindingContext as FoodItemsViewModel;

            if (viewModel != null)
            {
                if (viewModel.LoadFoods.CanExecute(null))
                    viewModel.LoadFoods.Execute(null);
            }

            FoodListView.RefreshCommand = viewModel.LoadFoods;
        }

        private async void FoodListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var selectedPie = e.SelectedItem as FoodItem;
            await Navigation.PushAsync(new FoodDetailPage(selectedPie));
        }
    }
}