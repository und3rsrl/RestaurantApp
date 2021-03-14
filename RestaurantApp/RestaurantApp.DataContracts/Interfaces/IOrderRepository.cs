using RestaurantApp.DataModel.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantApp.DataContracts.Interfaces
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<Order> GetActiveOrder(int id);
        Task<Order> GetUserActiveOrder(string userEmail);
        Task<List<Order>> GetUserPreviousOrders(string userEmail);
        Task<List<Order>> GetWaiterActiveOrders(string waiterEmail);
    }
}
