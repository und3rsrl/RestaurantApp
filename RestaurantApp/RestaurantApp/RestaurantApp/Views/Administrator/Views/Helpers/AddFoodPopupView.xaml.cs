using Plugin.Media;
using Plugin.Media.Abstractions;
using RestaurantApp.ViewModels;
using Rg.Plugins.Popup.Pages;
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

		public AddFoodPopupView ()
		{
			InitializeComponent ();
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var viewModel = BindingContext as AddFoodViewModel;

            if (viewModel != null)
            {
                if (viewModel.LoadCategories.CanExecute(null))
                    viewModel.LoadCategories.Execute(null);
            }
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

            FileImage.Source = ImageSource.FromStream(() =>
            {
                return _mediaFile.GetStream();
            });
        }
	}
}