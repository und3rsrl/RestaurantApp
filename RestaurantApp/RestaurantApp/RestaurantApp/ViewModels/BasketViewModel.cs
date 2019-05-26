﻿using MvvmHelpers;
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
                        if (string.IsNullOrEmpty(Settings.ActiveOrder))
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
                                Settings.ActiveOrder = response;
                                Settings.Bascket.Clear();
                                FoodItems.ReplaceRange(Settings.Bascket.AddedFoods);
                                ResetTotal?.Invoke(this, EventArgs.Empty);
                                Settings.PaymentNotSelected = true;
                                await Page.DisplayAlert("Basket", "Your order has been registered. Have a good meal", "Ok");
                            }
                        }
                        else
                        {
                            var activeOrder = await _ordersApiService.GetActiveOrder();

                            foreach (var food in FoodItems)
                            {
                                activeOrder.OrderItems.Add(food);
                            }

                            var message = await _ordersApiService.UpdateOrder(activeOrder.OrderId, activeOrder);

                            await Page.DisplayAlert("Basket", "Your order has been registered. Have a good meal", "Ok");
                        }
                    }
                });
            }
        }
    }
}
