using RestaurantApp.DataModel.Models;
using System.Threading.Tasks;

namespace RestaurantApp.DataContracts.Interfaces
{
    public interface IForgotPasswordRepository : IGenericRepository<ForgotPassword>
    {
        Task<ForgotPassword> GetLatestForgotPasswordEntry(string email, string code);
    }
}
