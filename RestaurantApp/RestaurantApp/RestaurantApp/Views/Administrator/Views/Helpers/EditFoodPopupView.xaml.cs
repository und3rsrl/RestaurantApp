using Plugin.Media;
using Plugin.Media.Abstractions;
using RestaurantApp.Models;
using RestaurantApp.Services;
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
    public partial class EditFoodPopupView : PopupPage
    {
        private MediaFile _mediaFile;

        public EditFoodPopupView(FoodItem foodItem, EventHandler refreshFoods, List<string> categories)
        {
            InitializeComponent();
            BindingContext = new EditFoodViewModel(this);
            BindData(foodItem, refreshFoods, categories);
        }

        private void BindData(FoodItem item, EventHandler refreshFoods, List<string> categories)
        {
            Name_Entry.Text = item.Name;
            Ingredients_Entry.Text = item.Ingredients;
            Price_Entry.Text = item.Price.ToString();
            category_picker.ItemsSource = categories;
            category_picker.SelectedItem = item.Category;
            FileImage.Source = item.ImageUrl;
            var viewModel = BindingContext as EditFoodViewModel;
            viewModel.Id = item.Id;
            viewModel.ImageUrl = item.ImageUrl;
            viewModel.RefreshFoods += refreshFoods;
            viewModel.SuccessfulEdit += OnSuccessfulEdit;
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

            var viewModel = BindingContext as EditFoodViewModel;
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

            var viewModel = BindingContext as EditFoodViewModel;
            viewModel.Image = _mediaFile;

            FileImage.Source = ImageSource.FromStream(() =>
            {
                return _mediaFile.GetStream();
            });
        }

        private void OnSuccessfulEdit(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync(true);
        }

        public void OnClose(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync(true);
        }
    }
}