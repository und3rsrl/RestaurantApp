using RestaurantApp.BusinessEntities.DTOs.Category;
using RestaurantApp.Common.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantApp.BusinessService.Interfaces
{
    public interface ICategoryBusinessService
    {
        Task<OperationResult> CreateCategory(CategoryDetails category);
        Task<OperationResult> DeleteCategory(int id);
        IEnumerable<CategoryDetails> GetAllCategories();
        CategoryDetails GetCategory(int id);
        OperationResult UpdateCategory(int id, CategoryDetails category);
    }
}
