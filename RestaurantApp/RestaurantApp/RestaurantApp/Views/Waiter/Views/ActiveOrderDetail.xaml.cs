using RestaurantApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RestaurantApp.Views.Waiter.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ActiveOrderDetail : ContentPage
	{        
		public ActiveOrderDetail (WaiterOrderInfo order)
		{
			InitializeComponent ();
            BindData(order);
        }

        private void BindData(WaiterOrderInfo order)
        {
            OrderListView.ItemsSource = order.OrderItems;
            Total_Label.Text = string.Concat("Total: ", order.Total, " Lei");
            Submitter_Label.Text = string.Concat("Submitter: ", order.Submitter);
            Tabel_Label.Text = string.Concat("Tabel: ", order.Table);
        }
	}
}