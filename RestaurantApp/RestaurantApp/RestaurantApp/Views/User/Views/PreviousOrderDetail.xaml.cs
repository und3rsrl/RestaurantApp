using RestaurantApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RestaurantApp.Views.User.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PreviousOrderDetail : ContentPage
	{
		public PreviousOrderDetail (PreviousOrder order)
		{
			InitializeComponent ();
            BindData(order);
        }

        private void BindData(PreviousOrder order)
        {
            OrderListView.ItemsSource = order.OrderItems;
            Total_Label.Text = string.Concat("Total: ", order.Total, " Lei");
        }
    }
}