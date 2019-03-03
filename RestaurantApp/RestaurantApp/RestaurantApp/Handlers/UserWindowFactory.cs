using RestaurantApp.DTOs;
using RestaurantApp.Views.Administrator;
using RestaurantApp.Views.User;
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
                    return new AdminMainPage();

                case "User":
                    return new UserMainPage();

                case "Waiter":
                    throw new NotImplementedException("This section is not completed.");

                default:
                    throw new InvalidOperationException("Unsupported role. Abort.");
            }
        }
    }
}
