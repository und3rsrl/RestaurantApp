using AutoMapper;
using RestaurantApp.BusinessEntities.DTOs.Category;
using RestaurantApp.BusinessService.Interfaces;
using RestaurantApp.Common.Enums;
using RestaurantApp.DataContracts.Interfaces;
using RestaurantApp.DataModel.Models;
using RestaurantApp.DataServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantApp.BusinessService.Implementation
{
    public class CategoryBusinessService : ICategoryBusinessService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryBusinessService
            (
                ICategoryRepository categoryRepository,
                IUnitOfWork unitOfWork,
                IMapper mapper
            )
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public IEnumerable<CategoryDetails> GetAllCategories()
        {
            var categories = _categoryRepository.GetAll();
            return _mapper.Map<IEnumerable<CategoryDetails>>(categories);

        }

        public CategoryDetails GetCategory(int id)
        {
            var category = _categoryRepository.GetById(id);
            return _mapper.Map<CategoryDetails>(category);
        }

        public OperationResult UpdateCategory(int id, CategoryDetails category)
        {
            var model = _mapper.Map<Category>(category);
            if (id != model.Id)
            {
                return OperationResult.Failed;
            }

            _categoryRepository.Update(model);
            _unitOfWork.CommitChangesAsync();

            return OperationResult.Succeeded;
        }

        public async Task<OperationResult> CreateCategory(CategoryDetails category)
        {
            try
            {
                var model = _mapper.Map<Category>(category);
                var existingCategory = await _categoryRepository.GetCategoryByName(category.Name);
                if (existingCategory != null)
                {
                    return OperationResult.Failed;
                }

                _categoryRepository.Add(model);
                await _unitOfWork.CommitChangesAsync();

                return OperationResult.Succeeded;
            }
            catch (Exception e)
            {
                return OperationResult.Failed;
            }
        }

        public async Task<OperationResult> DeleteCategory(int id)
        {
            var isCategoryExisting = await _categoryRepository.IsCategoryExisting(id);

            if (isCategoryExisting)
            {
                _categoryRepository.DeleteById(id);
                await _unitOfWork.CommitChangesAsync();

                return OperationResult.Succeeded;
            }

            return OperationResult.Failed;
        }
    }
}
