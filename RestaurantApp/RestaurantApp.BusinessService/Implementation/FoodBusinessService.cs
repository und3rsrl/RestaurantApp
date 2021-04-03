using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using RestaurantApp.BusinessEntities.DTOs.Food;
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
        private readonly IMapper _mapper;

        public FoodBusinessService
            (
                IUnitOfWork unitOfWork,
                IFoodRepository foodRepository,
                IMapper mapper,
                IHostingEnvironment hostingEnvironment
            )
        {
            _foodRepository = foodRepository;
            _hostingEnvironment = hostingEnvironment;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public IEnumerable<FoodDetails> GetAllFood()
        {
            var foods = _foodRepository.GetAll();
            return _mapper.Map<IEnumerable<FoodDetails>>(foods);
        }

        public FoodDetails GetFood(int id)
        {
            var food = _foodRepository.GetById(id);
            return _mapper.Map<FoodDetails>(food);
        }

        public async Task<OperationResult> UpdateFood(int id, FoodDetails food)
        {
            try
            {
                var model = _mapper.Map<Food>(food);

                if (id != model.Id)
                {
                    return OperationResult.Failed;
                }

                var existingFood = _foodRepository.GetById(id);

                if (existingFood.ImageUrl.Equals(model.ImageUrl, StringComparison.OrdinalIgnoreCase))
                {
                    string imageFullPath = string.Concat(_hostingEnvironment.WebRootPath, "\\FoodPhotos\\", existingFood.ImageUrl);
                    if (File.Exists(imageFullPath))
                    {
                        File.Delete(imageFullPath);
                    }
                }

                _foodRepository.Update(model);
                await _unitOfWork.CommitChangesAsync();

                return OperationResult.Succeeded;
            }
            catch (Exception)
            {
                return OperationResult.Failed;
            }
        }

        public async Task<OperationResult> CreateFood(FoodDetails food)
        {
            try
            {
                var model = _mapper.Map<Food>(food);
                _foodRepository.Add(model);
                await _unitOfWork.CommitChangesAsync();

                return OperationResult.Succeeded;
            }
            catch (Exception e)
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
