using MvvmHelpers;
using PayPal.Forms;
using PayPal.Forms.Abstractions;
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
        private WaitersApiService _waiterApiService = new WaitersApiService();
        private Order _order;

        public ActiveOrderViewModel()
        {
            OrderItems = new ObservableRangeCollection<OrderItem>();
        }

        public ActiveOrderViewModel(Page page)
        {
            OrderItems = new ObservableRangeCollection<OrderItem>();
        }

        public EventHandler NoActiveOrderUIHandler;

        public EventHandler HasActiveOrderUIHandler;

        public ObservableRangeCollection<OrderItem> OrderItems
        {
            get; private set;
        }

        public Page Page { get; set; }

        public bool PaymentNotSelected
        {
            get => Settings.PaymentNotSelected;

            set
            {
                Settings.PaymentNotSelected = value;
                OnPropertyChanged(nameof(PaymentNotSelected));
            }
        }

        public bool WaiterPaymentSelected
        {
            get => Settings.WaiterPaymentSelected;

            set
            {
                Settings.WaiterPaymentSelected = value;
                OnPropertyChanged(nameof(WaiterPaymentSelected));
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
                    var splits = Settings.ActiveOrder.Split('/');
                    var id = Convert.ToInt32(splits[splits.Length - 1]);
                    try
                    {
                        if (PaymentMethod.Contains("Waiter Payment"))
                        {
                            _ordersApiService.WaiterPay(id);
                            PaymentNotSelected = false;
                            WaiterPaymentSelected = true;
                        }
                        else if (PaymentMethod.Contains("Credit Card"))
                        {
                            PaymentNotSelected = false;

                            var result = await CrossPayPalManager.Current.Buy(new PayPalItem("Order-" + id, new Decimal(_order.Total), "RON"), new Decimal(0));
                            if (result.Status == PayPalStatus.Cancelled)
                            {
                                await Page.DisplayAlert("Payment", "Payment cancelled", "Ok");
                                Debug.WriteLine("Cancelled");
                            }
                            else if (result.Status == PayPalStatus.Error)
                            {
                                await Page.DisplayAlert("Payment", "Payment unsuccessful", "Ok");
                                Debug.WriteLine(result.ErrorMessage);
                            }
                            else if (result.Status == PayPalStatus.Successful)
                            {
                                await Page.DisplayAlert("Payment", "Payment successfully", "Ok");
                                Debug.WriteLine(result.ServerResponse.Response.Id);
                                _waiterApiService.PaidOrder(id);
                                await ExecuteGetActiveOrderCommand();
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        PaymentNotSelected = true;
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
                {
                    NoActiveOrderUIHandler?.Invoke(this, EventArgs.Empty);
                    WaiterPaymentSelected = false;
                }
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
