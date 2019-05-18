using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Threading.Tasks;
using Android;
using Plugin.CurrentActivity;
using Xamarin.Forms;
using Plugin.Toasts;
using PayPal.Forms;
using PayPal.Forms.Abstractions;
using Android.Content;

namespace RestaurantApp.Droid
{
    [Activity(Label = "RestaurantApp", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected async override void OnCreate(Bundle savedInstanceState)
        {
            await TryToGetPermissions();
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            Rg.Plugins.Popup.Popup.Init(this, savedInstanceState);
            //CrossCurrentActivity.Current.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            var config = new PayPalConfiguration(PayPalEnvironment.NoNetwork, "ARIHKnPqN07GY-CuoPFnZ6C82IXR9EpK5wFRQDaUv8qIfgFZZUyRM7CbpVL2xLYo3hhZPS2bpEukyYpo")
            {
                //If you want to accept credit cards
                AcceptCreditCards = false,
                //Your business name
                MerchantName = "Test Store",
                //Your privacy policy Url
                MerchantPrivacyPolicyUri = "https://www.example.com/privacy",
                //Your user agreement Url
                MerchantUserAgreementUri = "https://www.example.com/legal",
            };
            CrossPayPalManager.Init(config, this);

            global::Xamarin.FormsMaps.Init(this, savedInstanceState);
            //FFImageLoading.Forms.Platform.CachedImageRenderer.Init(false);
            DependencyService.Register<ToastNotification>();
            ToastNotification.Init(this);
            LoadApplication(new App());
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            PayPalManagerImplementation.Manager.OnActivityResult(requestCode, resultCode, data);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            PayPalManagerImplementation.Manager.Destroy();
        }

        #region RuntimePermissions

        async Task TryToGetPermissions()
        {
            if ((int)Build.VERSION.SdkInt >= 23)
            {
                await GetPermissionsAsync();
                return;
            }


        }

        const int RequestLocationId = 0;

        readonly string[] PermissionsGroupLocation =
            {
                            //TODO add more permissions
                            Manifest.Permission.AccessCoarseLocation,
                            Manifest.Permission.AccessFineLocation,
             };

        async Task GetPermissionsAsync()
        {
            const string permission = Manifest.Permission.AccessFineLocation;

            if (CheckSelfPermission(permission) == (int)Android.Content.PM.Permission.Granted)
            {
                //TODO change the message to show the permissions name
                Toast.MakeText(this, "Special permissions granted", ToastLength.Short).Show();
                return;
            }

            if (ShouldShowRequestPermissionRationale(permission))
            {
                //set alert for executing the task
                AlertDialog.Builder alert = new AlertDialog.Builder(this);
                alert.SetTitle("Permissions Needed");
                alert.SetMessage("The application need special permissions to continue");
                alert.SetPositiveButton("Request Permissions", (senderAlert, args) =>
                {
                    RequestPermissions(PermissionsGroupLocation, RequestLocationId);
                });

                alert.SetNegativeButton("Cancel", (senderAlert, args) =>
                {
                    Toast.MakeText(this, "Cancelled!", ToastLength.Short).Show();
                });

                Dialog dialog = alert.Create();
                dialog.Show();


                return;
            }

            RequestPermissions(PermissionsGroupLocation, RequestLocationId);

        }
        public override async void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            switch (requestCode)
            {
                case RequestLocationId:
                    {
                        if (grantResults[0] == (int)Android.Content.PM.Permission.Granted)
                        {
                            Toast.MakeText(this, "Special permissions granted", ToastLength.Short).Show();

                        }
                        else
                        {
                            //Permission Denied :(
                            Toast.MakeText(this, "Special permissions denied", ToastLength.Short).Show();

                        }
                    }
                    break;
            }

            Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            //base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        #endregion
    }
}