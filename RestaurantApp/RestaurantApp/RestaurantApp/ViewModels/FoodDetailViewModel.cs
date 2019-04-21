using MvvmHelpers;
using RestaurantApp.Helpers;
using RestaurantApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace RestaurantApp.ViewModels
{
    public class FoodDetailViewModel : BaseViewModel
    {
        public FoodDetailViewModel()
        {

        }

        public FoodDetailViewModel(Page page)
        {
            Page = page;
        }

        public int ProductId { get; set; }

        public string Name { get; set; }

        public int Amount { get; set; }

        public double Price { get; set; }

        public Page Page { get; }

        public ICommand AddToCartCommand
        {
            get
            {
                return new Command(async () =>
                {
                    if (Amount != 0)
                    {
                        OrderItem orderItem = new OrderItem()
                        {
                            ProductId = ProductId,
                            Amount = Amount,
                            Name = Name,
                            Price = Price,
                            Total = Amount * Price
                        };

                        Settings.Bascket.AddOrderItem(orderItem);
                    }
                    else
                        await Page.DisplayAlert("Add to Cart", "An amount greater than 0 is needed.", "Ok");
                });
            }
        }   
    }
}
