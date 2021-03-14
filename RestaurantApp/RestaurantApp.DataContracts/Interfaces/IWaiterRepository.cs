using RestaurantApp.DataModel.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantApp.DataContracts.Interfaces
{
    public interface IWaiterRepository : IGenericRepository<WaiterStatus>
    {
        Task<List<WaiterStatus>> GetActiveWaiters();
        Task<WaiterStatus> GetWaiterStatus(string waiterEmail);
    }
}
