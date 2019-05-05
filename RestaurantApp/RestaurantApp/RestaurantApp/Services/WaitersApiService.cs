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
    public class WaitersApiService : ApiService
    {
        public WaitersApiService()
            : base()
        {

        }

        public async Task<IEnumerable<WaiterItem>> GetWaiters()
        {
            var response = await HttpClient.GetAsync("Waiters");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<WaiterItem>>(content);
            }

            return null;
        }

        public async Task<string> AddWaiter(string email, string password)
        {
            var model = new RegisterBindingModel()
            {
                Email = email,
                Password = password,
                ConfirmPassword = password
            };

            var jsonObject = JsonConvert.SerializeObject(model);
            HttpContent content = new StringContent(jsonObject);

            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await HttpClient.PostAsync("Waiters", content);

            return await response.Content.ReadAsStringAsync();
        }

        public async void PaidOrder(int id)
        {
            await HttpClient.PostAsync("Orders/paidOrder/" + id, null);
        }

        public async Task<string> DeleteWaiter(string userId)
        {           
            var response = await HttpClient.DeleteAsync("Waiters/" + userId);

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<IEnumerable<WaiterOrderInfo>> GetOrders(string email)
        {
            var response = await HttpClient.GetAsync("Orders/activeWaiterOrders/" + email).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<WaiterOrderInfo>>(content);
            }

            return null;
        }
    }
}
