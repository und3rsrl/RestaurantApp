using RestaurantApp.Common.Enums;
using RestaurantApp.DataModel.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantApp.BusinessService.Interfaces
{
    public interface ICategoryBusinessService
    {
        Task<OperationResult> CreateCategory(Category category);
        Task<OperationResult> DeleteCategory(int id);
        IEnumerable<Category> GetAllCategories();
        Category GetCategory(int id);
        OperationResult UpdateCategory(int id, Category category);
    }
}
