using Newtonsoft.Json;
using RestaurantApp.Helpers;
using RestaurantApp.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantApp.Services
{
    public class OrdersApiService : ApiService
    {
        public OrdersApiService()
            : base()
        {
            
        }

        public async Task<string> PlaceOrders(Order order)
        {
            var json = JsonConvert.SerializeObject(order);
            HttpContent content = new StringContent(json);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await HttpClient.PostAsync("Orders", content);

            if (response.StatusCode == System.Net.HttpStatusCode.Created)
                return response.Headers.Location.ToString();

            return string.Empty;
        }

        public async Task<Order> GetActiveOrder()
        {
            if (string.IsNullOrEmpty(Settings.ActiveOrder))
                return null;

            var response = await HttpClient.GetAsync(Settings.ActiveOrder);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Order>(content);
            }

            return null;
        }

        public async void WaiterPay(int id)
        {
            await HttpClient.PostAsync("Orders/paidOrder/" + id, null);
        }
    }
}
