using Microsoft.AspNetCore.Mvc;
using RestaurantApp.BusinessEntities.DTOs.Category;
using RestaurantApp.BusinessService.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryBusinessService _categoryBusinessService;

        public CategoriesController(ICategoryBusinessService categoryBusinessService)
        {
            _categoryBusinessService = categoryBusinessService;
        }

        [HttpGet]
        public IEnumerable<CategoryDetails> GetCategories()
        {
            return _categoryBusinessService.GetAllCategories();
        }

        [HttpGet("{id}")]
        public CategoryDetails GetCategory([FromRoute] int id)
        {
            return _categoryBusinessService.GetCategory(id);
        }

        [HttpPut("{id}")]
        public IActionResult PutCategorie([FromRoute] int id, [FromBody] CategoryDetails category)
        {
            var result = _categoryBusinessService.UpdateCategory(id, category);
            if (result == Common.Enums.OperationResult.Succeeded)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> PostCategorie([FromBody] CategoryDetails category)
        {
            var result = await _categoryBusinessService.CreateCategory(category);
            if (result == Common.Enums.OperationResult.Succeeded)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategorie([FromRoute] int id)
        {
            var result = await _categoryBusinessService.DeleteCategory(id);
            if (result == Common.Enums.OperationResult.Succeeded)
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}