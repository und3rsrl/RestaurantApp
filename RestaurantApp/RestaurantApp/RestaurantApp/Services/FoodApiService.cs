using Newtonsoft.Json;
using Plugin.Media.Abstractions;
using RestaurantApp.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace RestaurantApp.Services
{
    public class FoodApiService : ApiService
    {
        public FoodApiService()
            : base()
        {

        }

        public async Task<IEnumerable<FoodItem>> GetFoods()
        {
            var response = await HttpClient.GetAsync("Foods");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<FoodItem>>(content);
            }

            return null;
        }

        public async Task<string> AddFood(string name, string ingredients, double price, int categoryId, MediaFile image)
        {
            MultipartFormDataContent mutipartContent = new MultipartFormDataContent
            {
                { new StreamContent(image.GetStream()), "photo", $"\"{image.Path}\"" }
            };

            var imageUploadResponse = await HttpClient.PostAsync("Foods/UploadPhoto", mutipartContent);

            var imageName = await imageUploadResponse.Content.ReadAsStringAsync();

            var model = new FoodItem()
            {
                Name = name,
                Ingredients = ingredients,
                Price = price,
                CategoryId = categoryId,
                ImageUrl = imageName
            };

            var jsonObject = JsonConvert.SerializeObject(model);
            HttpContent httpContent = new StringContent(jsonObject);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await HttpClient.PostAsync("Foods", httpContent);

            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                return await response.Content.ReadAsStringAsync();

            return string.Empty;
        }

        public async Task<string> EditFood(int id, FoodItem item, MediaFile image)
        {
            HttpResponseMessage imageUploadResponse;
            string imageName = string.Empty;

            if (image != null)
            {
                MultipartFormDataContent mutipartContent = new MultipartFormDataContent
                {
                    { new StreamContent(image.GetStream()), "photo", $"\"{image.Path}\"" }
                };

                imageUploadResponse = await HttpClient.PostAsync("Foods/UploadPhoto", mutipartContent);

                imageName = await imageUploadResponse.Content.ReadAsStringAsync();
            }

            var model = new FoodItem()
            {
                Id = id,
                Name = item.Name,
                Ingredients = item.Ingredients,
                Price = item.Price,
                Category = item.Category,
                ImageUrl = item.ImageUrl
            };

            if (!string.IsNullOrEmpty(imageName))
                model.ImageUrl = imageName;

            var jsonObject = JsonConvert.SerializeObject(model);
            HttpContent httpContent = new StringContent(jsonObject);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await HttpClient.PutAsync("Foods/" + id, httpContent);

            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                return await response.Content.ReadAsStringAsync();

            return string.Empty;
        }

        public async Task<string> DeleteFood(int id)
        {
            var response = await HttpClient.DeleteAsync("Foods/" + id);

            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                return await response.Content.ReadAsStringAsync();

            return string.Empty;
        }
    }
}
