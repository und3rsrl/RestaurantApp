using RestaurantApp.BusinessEntities.DTOs.Account;
using RestaurantApp.Common.Enums;
using System.Threading.Tasks;

namespace RestaurantApp.BusinessService.Interfaces
{
    public interface IAccountBusinessService
    {
        Task<string> Login(LoginDetails loginDetails);
        Task<OperationResult> PasswordRecovery(string email);
        Task<string> Register(RegisterDetails registerDetails);
        Task<OperationResult> UpdatePassword(PasswordChangeRequest passwordChangeRequest);
        Task<OperationResult> ValidateVerificationCode(string verificationCode, string email);
    }
}
