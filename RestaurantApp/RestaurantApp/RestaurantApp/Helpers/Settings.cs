﻿using Plugin.Settings;
using Plugin.Settings.Abstractions;
using RestaurantApp.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantApp.Helpers
{
    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }


        public static string AccessToken
        {
            get
            {
                return AppSettings.GetValueOrDefault("AccessToken", "");
            }

            set
            {
                AppSettings.AddOrUpdateValue("AccessToken", value);
            }
        }

        public static Cart Bascket
        {
            get
            {
                return Cart.Instance;
            }
        }
    }
}
