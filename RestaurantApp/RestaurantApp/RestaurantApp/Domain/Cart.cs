using RestaurantApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RestaurantApp.Domain
{
    public sealed class Cart
    {
        private static Cart _instance;
        private List<OrderItem> _orderItems;

        private Cart()
        {
            _orderItems = new List<OrderItem>();
        }

        public static Cart Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Cart();

                return _instance;
            }
        }

        public IEnumerable<OrderItem> AddedFoods => _orderItems;

        public void AddOrderItem(OrderItem item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            OrderItem existing = _orderItems.Where(x => x.ProductId == item.ProductId).FirstOrDefault();

            if (existing != null)
            {
                existing.Amount += item.Amount;
                existing.Total += item.Total;
            }
            else
            {
                _orderItems.Add(item);
            }
        }     
    }
}
