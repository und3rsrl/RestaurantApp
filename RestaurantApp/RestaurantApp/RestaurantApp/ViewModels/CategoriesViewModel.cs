using RestaurantApp.Models;
using RestaurantApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantApp.ViewModels
{
    public class CategoriesViewModel
    {
        private CategoriesApiService _categoriesApiService = new CategoriesApiService();

        public CategoriesViewModel()
        {

        }

        public IEnumerable<CategorieItem> Categories
        {
            get
            {
                return _categoriesApiService.GetCategories().Result;
            }
        }
    }
}
