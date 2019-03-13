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
            var result = await HttpClient.GetAsync("Categories");

            return null;
        }
    }
}
