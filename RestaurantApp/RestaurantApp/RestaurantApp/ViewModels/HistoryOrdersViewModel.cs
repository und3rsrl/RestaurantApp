using MvvmHelpers;
using RestaurantApp.Helpers;
using RestaurantApp.Models;
using RestaurantApp.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace RestaurantApp.ViewModels
{
    public class HistoryOrdersViewModel : BaseViewModel
    {
        private OrdersApiService _ordersApiService = new OrdersApiService();

        public HistoryOrdersViewModel()
        {
            PreviousOrders = new ObservableRangeCollection<PreviousOrder>();
        }

        public EventHandler NoOrdersUIHandler;

        public EventHandler HasOrdersUIHandler;

        public EventHandler EndRefreshHandler;

        public ObservableRangeCollection<PreviousOrder> PreviousOrders { get; private set; }

        public ICommand GetPreviousOrders
        {
            get
            {
                return new Command(async () => await ExecuteGetOrdersCommand());
            }
        }

        private async Task ExecuteGetOrdersCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                var orders = await _ordersApiService.GetPreviousOrders(Settings.UserName);

                if (orders == null || orders.Count() == 0)
                    NoOrdersUIHandler?.Invoke(this, EventArgs.Empty);
                else
                {
                    PreviousOrders.ReplaceRange(orders);
                    EndRefreshHandler?.Invoke(this, EventArgs.Empty);
                    HasOrdersUIHandler?.Invoke(this, EventArgs.Empty);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
