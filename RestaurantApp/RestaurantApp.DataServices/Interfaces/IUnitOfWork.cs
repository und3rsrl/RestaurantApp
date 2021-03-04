using System.Threading.Tasks;

namespace RestaurantApp.DataServices.Interfaces
{
    public interface IUnitOfWork
    {
        int CommitChanges();
        Task<int> CommitChangesAsync();
        void CreateTransaction();
        void RollbackTransaction();
        void CommitTransaction();
    }
}
