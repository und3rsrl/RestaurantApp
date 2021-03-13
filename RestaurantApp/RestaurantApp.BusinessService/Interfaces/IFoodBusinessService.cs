using Microsoft.AspNetCore.Http;
using RestaurantApp.Common.Enums;
using RestaurantApp.DataModel.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantApp.BusinessService.Interfaces
{
    public interface IFoodBusinessService
    {
        Task<OperationResult> CreateFood(Food food);
        Task<OperationResult> DeleteFood(int id);
        IEnumerable<Food> GetAllFood();
        Food GetFood(int id);
        Task<OperationResult> UpdateFood(int id, Food food);
        Task<string> UploadFoodImage(IFormFile foodPhoto);
    }
}
