using Newtonsoft.Json;
using RestaurantApp.Models;
using System;
using System.Collections;
using System.Collections.Generic;
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
            try
            {
                var response = await HttpClient.GetAsync("Categories").ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<CategorieItem>>(content);
                }
            }
            catch (Exception e)
            {
                
            }

            return null;
        }
    }
}
