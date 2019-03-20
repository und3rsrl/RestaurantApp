using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RestaurantApp.Views.Waiter
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class WaiterMasterDetailPage : MasterDetailPage
	{
		public WaiterMasterDetailPage ()
		{
			InitializeComponent ();
		}
	}
}