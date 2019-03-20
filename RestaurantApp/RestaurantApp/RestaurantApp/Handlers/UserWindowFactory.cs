using RestaurantApp.DTOs;
using RestaurantApp.Views.Administrator;
using RestaurantApp.Views.User;
using RestaurantApp.Views.Waiter;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace RestaurantApp.Handlers
{
    public static class UserWindowFactory
    {
        public static Page GenerateWindow(JWTPayloadDTO payload)
        {
            string userRole = payload.Role;

            switch (userRole)
            {
                case "Administrator":
                    return new AdminMasterDetailPage();

                case "User":
                    return new UserMasterDetailPage();

                case "Waiter":
                    return new WaiterMasterDetailPage();

                default:
                    throw new InvalidOperationException("Unsupported role. Abort.");
            }
        }
    }
}
