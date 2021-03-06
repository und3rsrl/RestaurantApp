using RestaurantApp.BusinessService.Interfaces;
using RestaurantApp.Common.Enums;
using RestaurantApp.DataContracts.Interfaces;
using RestaurantApp.DataModel.Models;
using RestaurantApp.DataServices.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantApp.BusinessService.Implementation
{
    public class CategoryBusinessService : ICategoryBusinessService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CategoryBusinessService
            (
                ICategoryRepository categoryRepository,
                IUnitOfWork unitOfWork
            )
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Category> GetAllCategories()
        {
            return _categoryRepository.GetAll();
        }

        public Category GetCategory(int id)
        {
            return _categoryRepository.GetById(id);
        }

        public OperationResult UpdateCategory(int id, Category category)
        {
            if (id != category.Id)
            {
                return OperationResult.Failed;
            }

            _categoryRepository.Update(category);
            _unitOfWork.CommitChangesAsync();

            return OperationResult.Succeeded;
        }

        public async Task<OperationResult> CreateCategory(Category category)
        {
            var existingCategory = await _categoryRepository.GetCategoryByName(category.Name);
            if (existingCategory != null)
            {
                return OperationResult.Failed;
            }

            _categoryRepository.Add(category);
            await _unitOfWork.CommitChangesAsync();

            return OperationResult.Succeeded;
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
