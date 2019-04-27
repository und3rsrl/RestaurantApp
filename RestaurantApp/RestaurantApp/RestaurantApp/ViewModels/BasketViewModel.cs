using MvvmHelpers;
using RestaurantApp.Helpers;
using RestaurantApp.Models;
using RestaurantApp.Services;
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
        }

        public IEnumerable<OrderItem> FoodItems => Settings.Bascket.AddedFoods;

        public int Table { get; set; }

        public double Total { get; set; }

        public Page Page { get; set; }

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
                        Order order = new Order()
                        {
                            SubmitDatetime = DateTime.Now,
                            Table = Table,
                            Submitter = Settings.UserName,
                            Total = Total,
                            OrderItems = FoodItems.ToList()
                        };

                        await _ordersApiService.PlaceOrders(order);
                        Debug.WriteLine("Comanda plasata.");
                    }
                });
            }
        }
    }
}
