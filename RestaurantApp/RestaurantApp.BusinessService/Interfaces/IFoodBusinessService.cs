using Microsoft.AspNetCore.Http;
using RestaurantApp.BusinessEntities.DTOs.Food;
using RestaurantApp.Common.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantApp.BusinessService.Interfaces
{
    public interface IFoodBusinessService
    {
        Task<OperationResult> CreateFood(FoodDetails food);
        Task<OperationResult> DeleteFood(int id);
        IEnumerable<FoodDetails> GetAllFood();
        FoodDetails GetFood(int id);
        Task<OperationResult> UpdateFood(int id, FoodDetails food);
        Task<string> UploadFoodImage(IFormFile foodPhoto);
    }
}
