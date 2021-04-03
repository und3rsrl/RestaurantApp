using RestaurantApp.BusinessEntities.DTOs.Order;
using RestaurantApp.BusinessEntities.DTOs.Waiter;
using RestaurantApp.Common.Enums;
using RestaurantApp.DataModel.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantApp.BusinessService.Interfaces
{
    public interface IOrderBusinessService
    {
        Task<OperationResult> CreateOrder(OrderDTO orderDetails);
        Task<OperationResult> DeleteOrder(int id);
        Task<Order> GetActiveOrder(int id);
        IEnumerable<Order> GetAllOrders();
        Task<Order> GetUserActiveOrder(string userEmail);
        Task<IEnumerable<PreviousOrderDetails>> GetUserPreviousOrders(string userEmail);
        Task<IEnumerable<WaiterOrderInfoDetails>> GetWaiterActiveOrders(string waiterEmail);
        Task<OperationResult> MarkAsPaid(int orderId);
        Task<OperationResult> MarkAsWaiterPayment(int orderId);
        Task<OperationResult> UpdateOrder(int id, Order order);
    }
}
