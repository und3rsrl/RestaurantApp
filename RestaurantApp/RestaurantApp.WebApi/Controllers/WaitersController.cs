using Microsoft.AspNetCore.Mvc;
using RestaurantApp.BusinessEntities.DTOs.Account;
using RestaurantApp.BusinessEntities.DTOs.Waiter;
using RestaurantApp.BusinessService.Interfaces;
using RestaurantApp.Common.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WaitersController : ControllerBase
    {
        private IWaiterBusinessService _waiterBusinessService;

        public WaitersController
            (
                IWaiterBusinessService waiterBusinessService
            )
        {
            _waiterBusinessService = waiterBusinessService;
        }

        [HttpGet]
        public async Task<IEnumerable<WaiterDTO>> GetWaiters()
        {
            return await _waiterBusinessService.GetWaiters();
        }

        [HttpPost]
        public async Task<IActionResult> AddWaiter([FromBody] RegisterDetails registerDetails)
        {
            var result = await _waiterBusinessService.AddWaiter(registerDetails);

            if (result == OperationResult.Succeeded)
            {
                return Ok();
            }

            return BadRequest("Account was not created.");
        }

        [HttpPost]
        [Route("setStatus")]
        public async Task<IActionResult> SetStatus([FromBody] string email)
        {
            var result = await _waiterBusinessService.SetStatus(email);

            if (result == OperationResult.Succeeded)
            {
                return Ok();
            }

            return BadRequest("Status was not set.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWaiter([FromRoute] string id)
        {
            var result = await _waiterBusinessService.DeleteWaiter(id);

            if (result == OperationResult.Succeeded)
            {
                return Ok("Account deleted successfully.");
            }

            return BadRequest("Account was not deleted.");
        }
    }
}