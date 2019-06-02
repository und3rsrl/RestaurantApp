using Android.Content;
using MvvmHelpers;
using RestaurantApp.Helpers;
using RestaurantApp.Models;
using RestaurantApp.Services;
using RestaurantApp.Views.User.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace RestaurantApp.ViewModels
{
    public class BasketViewModel : BaseViewModel
    {
        private OrdersApiService _ordersApiService = new OrdersApiService();

        public BasketViewModel()
        {

        }

        public BasketViewModel(Page page)
        {
            Page = page;
            FoodItems = new ObservableRangeCollection<OrderItem>();
            FoodItems.AddRange(Settings.Bascket.AddedFoods);
        }

        public ObservableRangeCollection<OrderItem> FoodItems
        {
            get; private set;
        }

        public int Table { get; set; }

        public double Total { get; set; }

        public Page Page { get; set; }

        public event EventHandler ResetTotal;
        public event EventHandler RecalculateTotal;
        public event EventHandler HideTableSelection;

        public ICommand HasActiveOrder
        {
            get
            {
                return new Command(async () =>
                {
                    var result = await _ordersApiService.HasActiveOrder();

                    if (result)
                        HideTableSelection?.Invoke(this, EventArgs.Empty);
                });
            }
        }

        public ICommand IncreaseAmount
        {
            get
            {
                return new Command<int>((int item) =>
                {
                    var orderItem = Settings.Bascket.AddedFoods.First(x => x.ProductId == item);
                    orderItem.Amount += 1;
                    orderItem.Total = orderItem.Price * orderItem.Amount;
                    ExecuteLoadFoodsCommand();
                    RecalculateTotal?.Invoke(this, EventArgs.Empty);
                });
            }
        }

        public ICommand DecreaseAmount
        {
            get
            {
                return new Command<int>((int item) =>
                {
                    var orderItem = Settings.Bascket.AddedFoods.First(x => x.ProductId == item);
                    if (orderItem.Amount > 1)
                    {
                        orderItem.Amount -= 1;
                        orderItem.Total = orderItem.Price * orderItem.Amount;
                        ExecuteLoadFoodsCommand();
                        RecalculateTotal?.Invoke(this, EventArgs.Empty);
                    }
                });
            }
        }

        public ICommand DeleteOrderItem
        {
            get
            {
                return new Command<OrderItem>((OrderItem item) =>
                {
                    Settings.Bascket.Delete(item);
                    ExecuteLoadFoodsCommand();
                    RecalculateTotal?.Invoke(this, EventArgs.Empty);
                });
            }
        }

        public ICommand PlaceOrderCommand
        {
            get
            {
                return new Command(async () =>
                {
                    if (FoodItems.Count() <= 0)
                    {
                        await Page.DisplayAlert("Basket", "You don't have food in your basket! Go to the Menu page.", "Ok");
                    }
                    else if (Table == 0)
                    {
                        await Page.DisplayAlert("Basket", "Please select you table.", "Ok");
                    }
                    else
                    {
                        var activeOrder = await _ordersApiService.GetActiveOrder();

                        if (activeOrder == null)
                        {
                            Order order = new Order()
                            {
                                SubmitDatetime = DateTime.Now,
                                Table = Table,
                                Submitter = Settings.UserName,
                                Total = Total,
                                OrderItems = FoodItems.ToList()
                            };

                            var response = await _ordersApiService.PlaceOrders(order);

                            if (string.IsNullOrEmpty(response))
                                await Page.DisplayAlert("Basket", "Something went wrong with your order. Please try again", "Ok");
                            else
                            {
                                Settings.Bascket.Clear();
                                FoodItems.ReplaceRange(Settings.Bascket.AddedFoods);
                                ResetTotal?.Invoke(this, EventArgs.Empty);
                                Settings.PaymentNotSelected = true;
                                await Page.DisplayAlert("Basket", "Your order has been registered. Have a good meal", "Ok");
                            }
                        }
                        else
                        {
                            UpdateOrder(activeOrder, FoodItems);
                            Settings.Bascket.Clear();
                            FoodItems.ReplaceRange(Settings.Bascket.AddedFoods);
                            ResetTotal?.Invoke(this, EventArgs.Empty);
                            Settings.PaymentNotSelected = true;
                            var message = await _ordersApiService.UpdateOrder(activeOrder.OrderId, activeOrder);
                            await Page.DisplayAlert("Basket", "Your order has been updated. Have a good meal", "Ok");
                        }
                    }
                });
            }
        }

        private void UpdateOrder(Order order, IEnumerable<OrderItem> newFoods)
        {
            foreach (var food in newFoods)
            {
                var foodItem = order.OrderItems.FirstOrDefault(x => x.ProductId == food.ProductId);

                if (foodItem == null)
                {
                    food.OrderItemId = 0;
                    order.OrderItems.Add(food);
                }             
                else
                    UpdateOrderItem(foodItem, food);
            }

            CalculateTotal(order);
        }

        private void UpdateOrderItem(OrderItem previousOrderItem, OrderItem newOrderItem)
        {
            previousOrderItem.Amount += newOrderItem.Amount;
            previousOrderItem.Total += newOrderItem.Total;
        }

        private void CalculateTotal(Order order)
        {
            var total = 0d;

            foreach (var item in order.OrderItems)
            {
                total += item.Total;
            }

            order.Total = total;
        }

        private void ExecuteLoadFoodsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                var items = Settings.Bascket.AddedFoods;

                FoodItems.ReplaceRange(items);
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
