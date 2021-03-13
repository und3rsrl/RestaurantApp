using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using RestaurantApp.BusinessService.Interfaces;
using RestaurantApp.Common.Enums;
using RestaurantApp.DataContracts.Interfaces;
using RestaurantApp.DataModel.Models;
using RestaurantApp.DataServices.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantApp.BusinessService.Implementation
{
    public class FoodBusinessService : IFoodBusinessService
    {
        private readonly IFoodRepository _foodRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHostingEnvironment _hostingEnvironment;

        public FoodBusinessService
            (
                IFoodRepository foodRepository,
                IHostingEnvironment hostingEnvironment
            )
        {
            _foodRepository = foodRepository;
            _hostingEnvironment = hostingEnvironment;
        }

        public IEnumerable<Food> GetAllFood()
        {
            return _foodRepository.GetAll();
        }

        public Food GetFood(int id)
        {
            return _foodRepository.GetById(id);
        }

        public async Task<OperationResult> UpdateFood(int id, Food food)
        {
            try
            {
                if (id != food.Id)
                {
                    return OperationResult.Failed;
                }

                var existingFood = _foodRepository.GetById(id);

                if (existingFood.ImageUrl.Equals(food.ImageUrl, StringComparison.OrdinalIgnoreCase))
                {
                    string imageFullPath = string.Concat(_hostingEnvironment.WebRootPath, "\\FoodPhotos\\", existingFood.ImageUrl);
                    if (File.Exists(imageFullPath))
                    {
                        File.Delete(imageFullPath);
                    }
                }

                _foodRepository.Update(food);
                await _unitOfWork.CommitChangesAsync();

                return OperationResult.Succeeded;
            }
            catch (Exception)
            {
                return OperationResult.Failed;
            }
        }

        public async Task<OperationResult> CreateFood(Food food)
        {
            try
            {
                _foodRepository.Add(food);
                await _unitOfWork.CommitChangesAsync();

                return OperationResult.Succeeded;
            }
            catch (Exception)
            {
                return OperationResult.Failed;
            }
        }

        public async Task<OperationResult> DeleteFood(int id)
        {
            try
            {
                var foodToBeDeleted = _foodRepository.GetById(id);

                if (foodToBeDeleted == null)
                {
                    return OperationResult.Failed;
                }

                string foodImagefullPath = string.Concat(_hostingEnvironment.WebRootPath, "\\FoodPhotos\\" + foodToBeDeleted.ImageUrl.Replace('/', '\\'));
                if (File.Exists(foodImagefullPath))
                {
                    File.Delete(foodImagefullPath);
                }

                _foodRepository.Delete(foodToBeDeleted);
                await _unitOfWork.CommitChangesAsync();

                return OperationResult.Succeeded;
            }
            catch (Exception)
            {
                return OperationResult.Failed;
            }
        }

        public async Task<string> UploadFoodImage(IFormFile foodPhoto)
        {
            var photoName = foodPhoto.FileName.Split('\\').LastOrDefault().Split('/').LastOrDefault();
            var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "FoodPhotos", photoName);

            if (foodPhoto.Length > 0)
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await foodPhoto.CopyToAsync(stream);
                }
            }

            return photoName;
        }
    }
}
