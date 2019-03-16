using Newtonsoft.Json;
using RestaurantApp.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantApp.Services
{
    public class CategoriesApiService : ApiService
    {
        public CategoriesApiService()
            : base()
        {

        }

        public async Task<IEnumerable<CategorieItem>> GetCategories()
        {
            var response = await HttpClient.GetAsync("Categories").ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<CategorieItem>>(content);
            }

            return null;
        }

        public async Task<string> AddCategorie(string name)
        {
            var model = new CategorieItem()
            {
                Name = name
            };

            var json = JsonConvert.SerializeObject(model);
            HttpContent content = new StringContent(json);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await HttpClient.PostAsync("Categories", content);

            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                return await response.Content.ReadAsStringAsync();

            return string.Empty;
        }

        public async Task<string> EditCategorie(int id, string name)
        {
            var model = new CategorieItem()
            {
                Id = id,
                Name = name
            };

            var json = JsonConvert.SerializeObject(model);
            HttpContent content = new StringContent(json);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await HttpClient.PutAsync("Categories/" + id, content);

            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                return await response.Content.ReadAsStringAsync();

            return string.Empty;
        }

        public async Task<string> DeleteCategorie(int id)
        {
            var response = await HttpClient.DeleteAsync("Categories/" + id);

            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                return await response.Content.ReadAsStringAsync();

            return string.Empty;
        }
    }
}
