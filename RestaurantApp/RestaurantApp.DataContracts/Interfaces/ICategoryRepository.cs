using RestaurantApp.DataModel.Models;
using System.Threading.Tasks;

namespace RestaurantApp.DataContracts.Interfaces
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<Category> GetCategoryByName(string categoryName);

        Task<bool> IsCategoryExisting(int id);
    }
}
