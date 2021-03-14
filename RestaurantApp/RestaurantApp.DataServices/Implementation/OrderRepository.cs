using Microsoft.EntityFrameworkCore;
using RestaurantApp.DataContracts.Interfaces;
using RestaurantApp.DataModel.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantApp.DataServices.Implementation
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(Entities dbContext) : base(dbContext)
        {
        }

        public Task<List<Order>> GetUserPreviousOrders(string userEmail)
        {
            return DbContext.Orders
                .Include(o => o.OrderItems)
                .Where(o => o.Submitter.Equals(userEmail) && o.IsPaid == true)
                .ToListAsync();
        }

        public Task<List<Order>> GetWaiterActiveOrders(string waiterEmail)
        {
            return DbContext.Orders
                .Include(o => o.OrderItems)
                .Where(o => o.Waiter.Equals(waiterEmail) && o.IsPaid == false)
                .ToListAsync();
        }

        public Task<Order> GetActiveOrder(int id)
        {
            return DbContext.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == id && o.IsPaid == false);
        }

        public Task<Order> GetUserActiveOrder(string userEmail)
        {
            return DbContext.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Submitter == userEmail && o.IsPaid == false);
        }

        protected override string TableName => "Orders";
    }
}
