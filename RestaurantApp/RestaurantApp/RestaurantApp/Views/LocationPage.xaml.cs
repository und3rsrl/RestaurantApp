using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace RestaurantApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LocationPage : ContentPage
	{
		public LocationPage ()
		{
			InitializeComponent ();
            MyMap.MoveToRegion(
                MapSpan.FromCenterAndRadius(
                new Position(44.319565, 23.802052), Distance.FromMiles(1)));
            var restaurantPosition = new Position(44.317544, 23.798815); // Latitude, Longitude
            var pin = new Pin
            {
                Type = PinType.Place,
                Position = restaurantPosition,
                Label = "Restaurant"
            };
            MyMap.Pins.Add(pin);
        }
	}
}