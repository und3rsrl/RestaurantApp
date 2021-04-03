using RestaurantApp.BusinessEntities.DTOs.Account;
using RestaurantApp.BusinessEntities.DTOs.Waiter;
using RestaurantApp.Common.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantApp.BusinessService.Interfaces
{
    public interface IWaiterBusinessService
    {
        Task<OperationResult> AddWaiter(RegisterDetails registerDetails);
        Task<OperationResult> DeleteWaiter(string id);
        Task<IEnumerable<WaiterDetails>> GetWaiters();
        Task<OperationResult> SetStatus(string email);
    }
}
