using Microsoft.EntityFrameworkCore;
using RestaurantApp.DataContracts.Interfaces;
using RestaurantApp.DataModel.Models;
using System.Threading.Tasks;

namespace RestaurantApp.DataServices.Implementation
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(Entities dbContext) : base(dbContext)
        {
        }

        public Task<Category> GetCategoryByName(string categoryName)
        {
            return DbContext.Categories.FirstOrDefaultAsync(c => c.Name.ToLower() == categoryName.ToLower());
        }

        public Task<bool> IsCategoryExisting(int id)
        {
            return DbContext.Categories.AnyAsync(c => c.Id == id);
        }

        protected override string TableName => "Categories";
    }
}
