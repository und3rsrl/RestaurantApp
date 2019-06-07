using Newtonsoft.Json;
using RestaurantApp.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using RestaurantApp.Helpers;
using System.Threading.Tasks;

namespace RestaurantApp.Services
{
    public class CurrencyConverterService
    {
        public const string CURRENCY_API_URL = "http://api.exchangeratesapi.io/";
        public const string CURRENCY_API_URL_ALTERNATIVE = "https://api.ratesapi.io/api/";

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
            if (IsCurrencyRateOutdated())
            {
                var currencyRate = await GetCurrencyRate();

                if (currencyRate != 0)
                {
                    Settings.CurrencyRate = await GetCurrencyRate();
                }
                else
                {
                    _httpClient.BaseAddress = new Uri(CURRENCY_API_URL_ALTERNATIVE);
                    Settings.CurrencyRate = await GetCurrencyRate();
                }          
            }

            return RON / Settings.CurrencyRate;
        }

        private bool IsCurrencyRateOutdated()
        {
            if (Settings.CurrencyRateDate.AddDays(7) <= DateTime.Now)
                return true;

            return false;
        }

        private async Task<double> GetCurrencyRate()
        {
            var response = await _httpClient.GetAsync("latest?base=USD&symbols=RON").ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var currencyResponse = JsonConvert.DeserializeObject<CurrencyResponse>(content);

                return currencyResponse.Rates["RON"];
            }

            return 0;
        }
    }
}
