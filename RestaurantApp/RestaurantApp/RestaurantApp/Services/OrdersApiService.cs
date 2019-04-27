using Newtonsoft.Json;
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

            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                return await response.Content.ReadAsStringAsync();

            return string.Empty;
        }
    }
}
