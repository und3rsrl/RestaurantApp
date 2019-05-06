using MvvmHelpers;
using RestaurantApp.Helpers;
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

        public bool PaymentNotSelected
        {
            get => Settings.PaymentNotSelected;

            set
            {
                Settings.PaymentNotSelected = value;
                OnPropertyChanged(nameof(PaymentNotSelected));
            }
        }

        public string PaymentMethod { get; set; }

        public ICommand GetActiveOrder
        {
            get
            {
                return new Command(async () => await ExecuteGetActiveOrderCommand());
            }
        }

        public ICommand PayOrderCommand
        {
            get
            {
                return new Command(async () => 
                {
                    if (PaymentMethod.Contains("Waiter Payment"))
                    {
                        var splits = Settings.ActiveOrder.Split('/');

                        var id = Convert.ToInt32(splits[splits.Length - 1]);

                        _ordersApiService.WaiterPay(id);
                        PaymentNotSelected = false;
                    }
                    else if(PaymentMethod.Contains("Credit Card"))
                    {
                        PaymentNotSelected = false;
                    }
                });
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
                    OrderItems.ReplaceRange(_order.OrderItems);
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
