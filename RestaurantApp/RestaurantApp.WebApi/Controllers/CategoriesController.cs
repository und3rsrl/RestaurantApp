using Microsoft.AspNetCore.Mvc;
using RestaurantApp.BusinessService.Interfaces;
using RestaurantApp.DataModel.Models;
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

        // GET: api/Categories
        [HttpGet]
        public IEnumerable<Category> GetCategories()
        {
            return _categoryBusinessService.GetAllCategories();
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public Category GetCategory([FromRoute] int id)
        {
            return _categoryBusinessService.GetCategory(id);
        }

        // PUT: api/Categories/5
        [HttpPut("{id}")]
        public IActionResult PutCategorie([FromRoute] int id, [FromBody] Category category)
        {
            var result = _categoryBusinessService.UpdateCategory(id, category);
            if (result == Common.Enums.OperationResult.Succeeded)
            {
                return Ok();
            }

            return BadRequest();
        }

        // POST: api/Categories
        [HttpPost]
        public async Task<IActionResult> PostCategorie([FromBody] Category category)
        {
            var result = await _categoryBusinessService.CreateCategory(category);
            if (result == Common.Enums.OperationResult.Succeeded)
            {
                return Ok();
            }

            return BadRequest();
        }

        // DELETE: api/Categories/5
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