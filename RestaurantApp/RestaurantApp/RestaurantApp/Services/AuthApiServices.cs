using Newtonsoft.Json;
using RestaurantApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantApp.Services
{
    public class AuthApiServices : ApiService
    {
        public AuthApiServices()
            : base()
        {

        }

        public async Task<string> RegisterAsync(string email, string password, string confirmPassword)
        {
            var model = new RegisterBindingModel()
            {
                Email = email,
                Password = password,
                ConfirmPassword = confirmPassword
            };

            var json = JsonConvert.SerializeObject(model);

            HttpContent content = new StringContent(json);

            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await HttpClient.PostAsync("Account/Register", content);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return await response.Content.ReadAsStringAsync();
            else
                return "NotCreated";
        }

        public async Task<string> LoginAsync(string userName, string password)
        {
            var model = new LoginBindingModel()
            {
                Email = userName,
                Password = password,
            };

            var json = JsonConvert.SerializeObject(model);

            HttpContent content = new StringContent(json);

            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");


            var response = await HttpClient.PostAsync("Account/Login", content);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return "Unauthorized";

            return await response.Content.ReadAsStringAsync();
        }
    }
}
