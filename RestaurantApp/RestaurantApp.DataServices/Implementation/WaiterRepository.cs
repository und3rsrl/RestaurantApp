using Microsoft.EntityFrameworkCore;
using RestaurantApp.DataContracts.Interfaces;
using RestaurantApp.DataModel.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantApp.DataServices.Implementation
{
    public class WaiterRepository : GenericRepository<WaiterStatus>, IWaiterRepository
    {
        public WaiterRepository(Entities dbContext) : base(dbContext)
        {
        }

        public Task<WaiterStatus> GetWaiterStatus(string waiterEmail)
        {
            return DbContext.WaitersStatus
                .FirstOrDefaultAsync(ws => ws.Waiter == waiterEmail);
        }


        public Task<List<WaiterStatus>> GetActiveWaiters()
        {
            return DbContext.WaitersStatus
                .Where(w => w.IsActive)
                .ToListAsync();
        }

        protected override string TableName => "WaiterStatuses";
    }
}
