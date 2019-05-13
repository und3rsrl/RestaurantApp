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
    public class WaiterActiveOrdersViewModel : BaseViewModel
    {
        private WaitersApiService _waitersApiService = new WaitersApiService();

        public WaiterActiveOrdersViewModel()
        {
            Orders = new ObservableRangeCollection<WaiterOrderInfo>();
        }

        public EventHandler NoOrdersUIHandler;

        public EventHandler HasOrdersUIHandler;

        public EventHandler EndRefreshHandler;

        public ObservableRangeCollection<WaiterOrderInfo> Orders
        {
            get; private set;
        }

        public ICommand GetOrders
        {
            get
            {
                return new Command(async () => await ExecuteGetOrdersCommand());
            }
        }

        public ICommand PaidOrder
        {
            get
            {
                return new Command<int>(async (id) =>
                {
                    _waitersApiService.PaidOrder(id);
                    await ExecuteGetOrdersCommand();
                });
            }
        }

        private async Task ExecuteGetOrdersCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                var orders = await _waitersApiService.GetOrders(Settings.UserName);

                if (orders == null || orders.Count() == 0)
                    NoOrdersUIHandler?.Invoke(this, EventArgs.Empty);
                else
                {
                    Orders.ReplaceRange(orders);
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
