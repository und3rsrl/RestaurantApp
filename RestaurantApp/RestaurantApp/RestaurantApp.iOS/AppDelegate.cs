using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using KeyboardOverlap.Forms.Plugin.iOSUnified;
using PayPal.Forms;
using PayPal.Forms.Abstractions;
using Plugin.Toasts;
using UIKit;
using UserNotifications;
using Xamarin.Forms;

namespace RestaurantApp.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();

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

            CrossPayPalManager.Init(config);

            Rg.Plugins.Popup.Popup.Init();
            Xamarin.FormsMaps.Init();
            KeyboardOverlapRenderer.Init();
            //FFImageLoading.Forms.Platform.CachedImageRenderer.Init();
            DependencyService.Register<ToastNotification>();
            ToastNotification.Init();
            LoadApplication(new App());

            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
                // Request Permissions
                UNUserNotificationCenter.Current.RequestAuthorization(UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound, (granted, error) =>
                {
                    // Do something if needed
                });
            }
            else if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                var notificationSettings = UIUserNotificationSettings.GetSettingsForTypes(
                UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound, null);

                app.RegisterUserNotificationSettings(notificationSettings);
            }

            return base.FinishedLaunching(app, options);
        }
    }
}
