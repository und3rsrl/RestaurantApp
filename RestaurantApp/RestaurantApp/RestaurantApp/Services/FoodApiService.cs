using Newtonsoft.Json;
using Plugin.Media.Abstractions;
using RestaurantApp.DTOs;
using RestaurantApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantApp.Services
{
    public class FoodApiService : ApiService
    {
        public async Task<string> AddFood(string name, string ingredients, double price, string categorie, MediaFile image)
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
                Category = categorie,
                ImageUrl = "FoodPhotos/" + imageName
            };

            var jsonObject = JsonConvert.SerializeObject(model);
            HttpContent httpContent = new StringContent(jsonObject);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await HttpClient.PostAsync("Foods", httpContent);

            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                return await response.Content.ReadAsStringAsync();

            return string.Empty;
        }
    }
}
