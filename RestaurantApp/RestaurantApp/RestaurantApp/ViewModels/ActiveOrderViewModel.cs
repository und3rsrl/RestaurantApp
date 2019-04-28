using MvvmHelpers;
using RestaurantApp.Models;
using RestaurantApp.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace RestaurantApp.ViewModels
{
    public class ActiveOrderViewModel : BaseViewModel
    {
        private OrdersApiService _ordersApiService = new OrdersApiService();
        private Order _order;

        public ActiveOrderViewModel()
        {
            OrderItems = new ObservableRangeCollection<OrderItem>();
        }

        public EventHandler NoActiveOrderUIHandler;

        public EventHandler HasActiveOrderUIHandler;

        public ObservableRangeCollection<OrderItem> OrderItems
        {
            get; private set;
        }

        public ICommand GetActiveOrder
        {
            get
            {
                return new Command(async () => await ExecuteGetActiveOrderCommand());
            }
        }

        private async Task ExecuteGetActiveOrderCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                _order = await _ordersApiService.GetActiveOrder();

                if (_order == null)
                    NoActiveOrderUIHandler?.Invoke(this, EventArgs.Empty);
                else
                {
                    OrderItems.AddRange(_order.OrderItems);
                    HasActiveOrderUIHandler?.Invoke(this, EventArgs.Empty);
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
