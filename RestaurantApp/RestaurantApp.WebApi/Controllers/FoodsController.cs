using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantApp.BusinessEntities.DTOs.Food;
using RestaurantApp.BusinessService.Interfaces;
using RestaurantApp.Common.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodsController : ControllerBase
    {
        private readonly IFoodBusinessService _foodBusinessService;

        public FoodsController
            (
                IFoodBusinessService foodBusinessService
            )
        {
            _foodBusinessService = foodBusinessService;
        }

        [HttpGet]
        public IEnumerable<FoodDetails> GetFood()
        {
            return _foodBusinessService.GetAllFood();
        }

        [HttpGet("{id}")]
        public FoodDetails GetFood([FromRoute] int id)
        {
            return _foodBusinessService.GetFood(id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutFood([FromRoute] int id, [FromBody] FoodDetails food)
        {
            var result = await _foodBusinessService.UpdateFood(id, food);

            if (result == OperationResult.Succeeded)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> PostFood([FromBody] FoodDetails food)
        {
            var result = await _foodBusinessService.CreateFood(food);

            if (result == OperationResult.Succeeded)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFood([FromRoute] int id)
        {
            var result = await _foodBusinessService.DeleteFood(id);

            if (result == OperationResult.Succeeded)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpPost]
        [Route("UploadPhoto")]
        public async Task<IActionResult> UploadPhoto(IFormFile photo)
        {
            var fileName = await _foodBusinessService.UploadFoodImage(photo);
            return Ok(fileName);
        }
    }
}