using Newtonsoft.Json;
using RestaurantApp.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantApp.Services
{
    public class CurrencyConverterService
    {
        public const string CURRENCY_API_URL = "http://api.exchangeratesapi.io/";

        private HttpClient _httpClient;

        public CurrencyConverterService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(CURRENCY_API_URL)
            };
        }

        public async Task<double> ConvertRONtoUSD(double RON)
        {
            var response = await _httpClient.GetAsync("latest?symbols=RON&base=USD").ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var currencyResponse = JsonConvert.DeserializeObject<CurrencyResponse>(content);

                return RON / currencyResponse.Rates["RON"];
            }

            return 0;
        }
    }
}
