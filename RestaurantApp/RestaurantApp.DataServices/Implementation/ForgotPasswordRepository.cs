using Microsoft.EntityFrameworkCore;
using RestaurantApp.DataContracts.Interfaces;
using RestaurantApp.DataModel.Models;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantApp.DataServices.Implementation
{
    public class ForgotPasswordRepository : GenericRepository<ForgotPassword>, IForgotPasswordRepository
    {
        public ForgotPasswordRepository(Entities dbContext) : base(dbContext)
        {
        }

        public Task<ForgotPassword> GetLatestForgotPasswordEntry(string email, string code)
        {
            return DbContext.ForgotPassword
                .OrderByDescending(fp => fp.CreatedAt)
                .FirstOrDefaultAsync(fp => fp.Email == email && fp.Code == code);
        }

        protected override string TableName => "ForgotPassword";
    }
}
