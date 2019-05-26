using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantApp.WebApi.DTOs;
using RestaurantApp.WebApi.Entities;
using RestaurantApp.WebApi.Models;

namespace RestaurantApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Orders
        [HttpGet]
        public IEnumerable<Order> GetOrders()
        {
            return _context.Orders;
        }

        // GET: api/Orders/user@restaurant.com
        [HttpGet("userPreviousOrders/{email}")]
        public IEnumerable<PreviousOrderDTO> GetOrders([FromRoute] string email)
        {
            var orders = _context.Orders.Include(x => x.OrderItems).Where(x => x.Submitter.Equals(email) && x.IsPaid == true);

            List<PreviousOrderDTO> previousOrders = new List<PreviousOrderDTO>();
            foreach (var order in orders)
            {
                var previousOrder = new PreviousOrderDTO()
                {
                    SubmitDate = order.SubmitDateTime,
                    OrderItems = order.OrderItems,
                    Table = order.Table,
                    Total = order.Total
                };

                previousOrders.Add(previousOrder);
            }

            return previousOrders;
        }

        [HttpGet("activeWaiterOrders/{email}")]
        public IEnumerable<WaiterOrderInfoDTO> GetWaiterActiveOrders([FromRoute] string email)
        {
            var orders =_context.Orders.Include(x => x.OrderItems).Where(x => x.Waiter.Equals(email) && x.IsPaid == false);

            List<WaiterOrderInfoDTO> waiterOrdersInfosList = new List<WaiterOrderInfoDTO>();

            foreach (Order order in orders)
            {
                WaiterOrderInfoDTO orderInfoDTO = new WaiterOrderInfoDTO()
                {
                    Id = order.OrderId,
                    Submitter = order.Submitter,
                    Table = order.Table,
                    Total = order.Total,
                    WaiterPayment = order.WaiterPayment,
                    OrderItems = order.OrderItems
                };

                waiterOrdersInfosList.Add(orderInfoDTO);
            }

            return waiterOrdersInfosList;
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var order = await _context.Orders.Include(x => x.OrderItems).FirstOrDefaultAsync(i => i.OrderId == id && i.IsPaid == false);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        // PUT: api/Orders/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder([FromRoute] int id, [FromBody] Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != order.OrderId)
            {
                return BadRequest();
            }

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost("paidOrder/{id}")]
        public async Task<IActionResult> PaidOrder([FromRoute] int id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
                return BadRequest();

            order.IsPaid = true;

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost("waiterPayment/{id}")]
        public async Task<IActionResult> WaiterPaymentOrder([FromRoute] int id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
                return BadRequest();

            order.WaiterPayment = true;

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Orders
        [HttpPost]
        public async Task<IActionResult> PostOrder([FromBody] OrderDTO orderDetails)
        {
            Order order = new Order()
            {
                IsPaid = false,
                SubmitDateTime = orderDetails.SubmiteDatetime,
                Submitter = orderDetails.Submitter,
                Total = orderDetails.Total,
                Table = orderDetails.Table
            };

            var waiters = _context.WaitersStatus.Where(x => x.IsActive == true).ToList();

            Random random = new Random();
            var waiterIndex = random.Next(waiters.Count());
            order.Waiter = waiters[waiterIndex].Waiter;

            order.OrderItems = new List<OrderItem>();
            foreach (OrderItemDTO orderItemDetails in orderDetails.OrderItems)
            {
                OrderItem orderItem = new OrderItem()
                {
                    Amount = orderItemDetails.Amount,
                    Name = orderItemDetails.Name,
                    Price = orderItemDetails.Price,
                    ProductId = orderItemDetails.ProductId,
                    Total = orderItemDetails.Total,
                };

                order.OrderItems.Add(orderItem);
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrder", new { id = order.OrderId }, order);
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return Ok(order);
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }
    }
}