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

        public async Task<bool> HasActiveOrder()
        {
            var result = await GetActiveOrder().ConfigureAwait(false);

            return result != null;
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
            var response = await HttpClient.GetAsync("Orders/userActiveOrder/" + Settings.UserName);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return null;
  
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Order>(content); 
        }

        public async Task<string> UpdateOrder(int id, Order order)
        {
            var json = JsonConvert.SerializeObject(order);
            HttpContent content = new StringContent(json);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await HttpClient.PutAsync("Orders/" + id, content);

            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                return "Order successfuly updated";

            return "Something went wrong with the order.";
        }

        public async void WaiterPay(int id)
        {
            await HttpClient.PostAsync("Orders/waiterPayment/" + id, null);
        }

        public async Task<IEnumerable<PreviousOrder>> GetPreviousOrders(string email)
        {
            var response = await HttpClient.GetAsync("Orders/userPreviousOrders/" + email).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<PreviousOrder>>(content);
            }

            return null;
        }
    }
}
