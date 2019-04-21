using MvvmHelpers;
using RestaurantApp.Helpers;
using RestaurantApp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantApp.ViewModels
{
    public class BasketViewModel : BaseViewModel
    {
        public IEnumerable<OrderItem> FoodItems => Settings.Bascket.AddedFoods;
    }
}
