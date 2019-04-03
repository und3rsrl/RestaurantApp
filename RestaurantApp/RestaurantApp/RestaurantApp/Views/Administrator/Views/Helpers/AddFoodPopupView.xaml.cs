using Plugin.Media;
using Plugin.Media.Abstractions;
using RestaurantApp.ViewModels;
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
	public partial class AddFoodPopupView : PopupPage
	{
        private MediaFile _mediaFile;

		public AddFoodPopupView (EventHandler refreshFood, string selectedCategorie, List<string> categories)
		{
			InitializeComponent ();

            BindData(refreshFood, selectedCategorie, categories);
        }

        private void BindData(EventHandler refreshFoods, string selectedCategorie, List<string> categories)
        {
            var viewModel = BindingContext as AddFoodViewModel;
            viewModel.RefreshFoods += refreshFoods;
            category_picker.ItemsSource = categories;
            category_picker.SelectedItem = selectedCategorie;
        }

        private async void PickPhoto_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("No PickPhoto", "No PickPhoto available.", "OK");
                return;
            }

            _mediaFile = await CrossMedia.Current.PickPhotoAsync();

            if (_mediaFile == null)
                return;

            var viewModel = BindingContext as AddFoodViewModel;

            viewModel.Image = _mediaFile;

            FileImage.Source = ImageSource.FromStream(() =>
            {
                return _mediaFile.GetStream();
            });
        }

        private async void TakePhoto_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", "No camera available.", "OK");
                return;
            }

            _mediaFile = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions()
            {
                Directory = "Food",
                Name = $"{DateTime.UtcNow}.jpg"
            });

            if (_mediaFile == null)
                return;

            var viewModel = BindingContext as AddFoodViewModel;
            viewModel.Image = _mediaFile;

            FileImage.Source = ImageSource.FromStream(() =>
            {
                return _mediaFile.GetStream();
            });
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync(true);
        }
    }
}