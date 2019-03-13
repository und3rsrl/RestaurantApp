using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace RestaurantApp.Services
{
    public abstract class ApiService
    {
        protected const string BASE_API_URL = "http://192.168.1.5:50915/api/";

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
