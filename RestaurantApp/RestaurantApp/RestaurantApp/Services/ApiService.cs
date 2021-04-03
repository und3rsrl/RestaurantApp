using System;
using System.Net.Http;

namespace RestaurantApp.Services
{
    public abstract class ApiService
    {
        public const string BASE_API_URL = "http://192.168.1.207:5000/api/";
        public const string BASE_SERVER_IMAGE_URL = "http://192.168.1.207:5000/FoodPhotos/";

        private HttpClient _httpClient;

        public ApiService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(BASE_API_URL)
            };
        }

        protected HttpClient HttpClient => _httpClient;

    }
}
