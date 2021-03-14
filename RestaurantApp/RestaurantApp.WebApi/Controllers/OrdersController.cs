using Microsoft.AspNetCore.Mvc;
using RestaurantApp.BusinessEntities.DTOs.Order;
using RestaurantApp.BusinessEntities.DTOs.Waiter;
using RestaurantApp.BusinessService.Interfaces;
using RestaurantApp.Common.Enums;
using RestaurantApp.DataModel.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderBusinessService _orderBusinessService;

        public OrdersController
            (
                IOrderBusinessService orderBusinessService
            )
        {
            _orderBusinessService = orderBusinessService;
        }

        [HttpGet]
        public IEnumerable<Order> GetOrders()
        {
            return _orderBusinessService.GetAllOrders();
        }

        // GET: api/Orders/user@restaurant.com
        [HttpGet("userPreviousOrders/{email}")]
        public async Task<IEnumerable<PreviousOrderDTO>> GetUserPreviousOrders([FromRoute] string email)
        {
            return await _orderBusinessService.GetUserPreviousOrders(email);
        }

        [HttpGet("activeWaiterOrders/{email}")]
        public async Task<IEnumerable<WaiterOrderInfoDTO>> GetWaiterActiveOrders([FromRoute] string email)
        {
            return await _orderBusinessService.GetWaiterActiveOrders(email);
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<Order> GetOrder([FromRoute] int id)
        {
            return await _orderBusinessService.GetActiveOrder(id);
        }

        // GET: api/Orders/5
        [HttpGet("userActiveOrder/{email}")]
        public async Task<Order> GetActiveOrder([FromRoute] string email)
        {
            return await _orderBusinessService.GetUserActiveOrder(email);
        }

        // PUT: api/Orders/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder([FromRoute] int id, [FromBody] Order order)
        {
            var result = await _orderBusinessService.UpdateOrder(id, order);

            if (result == OperationResult.Succeeded)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpPost("paidOrder/{id}")]
        public async Task<IActionResult> MarkAsPaid([FromRoute] int id)
        {
            var result = await _orderBusinessService.MarkAsPaid(id);

            if (result == OperationResult.Succeeded)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpPost("waiterPayment/{id}")]
        public async Task<IActionResult> MarkAsWaiterPayment([FromRoute] int id)
        {
            var result = await _orderBusinessService.MarkAsWaiterPayment(id);

            if (result == OperationResult.Succeeded)
            {
                return Ok();
            }

            return BadRequest();
        }

        // POST: api/Orders
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderDTO orderDetails)
        {
            var result = await _orderBusinessService.CreateOrder(orderDetails);

            if (result == OperationResult.Succeeded)
            {
                return Ok();
            }

            return BadRequest();
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder([FromRoute] int id)
        {
            var result = await _orderBusinessService.DeleteOrder(id);

            if (result == OperationResult.Succeeded)
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}