using AutoMapper;
using Microsoft.AspNetCore.Identity;
using RestaurantApp.BusinessEntities.DTOs.Account;
using RestaurantApp.BusinessEntities.DTOs.Waiter;
using RestaurantApp.BusinessService.Interfaces;
using RestaurantApp.Common.Enums;
using RestaurantApp.DataContracts.Interfaces;
using RestaurantApp.DataServices.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantApp.BusinessService.Implementation
{
    public class WaiterBusinessService : IWaiterBusinessService
    {
        private readonly IWaiterRepository _waiterRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;

        public WaiterBusinessService
            (
                IWaiterRepository waiterRepository,
                IUnitOfWork unitOfWork,
                IMapper mapper,
                UserManager<IdentityUser> userManager
            )
        {
            _waiterRepository = waiterRepository;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<IEnumerable<WaiterDetails>> GetWaiters()
        {

            var waiters = await _userManager.GetUsersInRoleAsync("Waiter");

            List<WaiterDetails> waitersDTO = new List<WaiterDetails>();

            foreach (var waiter in waiters)
            {
                waitersDTO.Add(new WaiterDetails
                {
                    UserId = waiter.Id,
                    Email = waiter.Email
                });
            }

            return waitersDTO;
        }

        public async Task<OperationResult> AddWaiter(RegisterDetails registerDetails)
        {
            var waiter = new IdentityUser
            {
                UserName = registerDetails.Email,
                Email = registerDetails.Email
            };

            var result = await _userManager.CreateAsync(waiter, registerDetails.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(waiter, "Waiter");

                return OperationResult.Succeeded;
            }

            return OperationResult.Failed;
        }

        public async Task<OperationResult> DeleteWaiter(string id)
        {
            if (id == null)
            {
                return OperationResult.Failed;
            }

            var waiter = await _userManager.FindByIdAsync(id);
            var logins = await _userManager.GetLoginsAsync(waiter);
            var rolesForUser = await _userManager.GetRolesAsync(waiter);

            foreach (var login in logins)
            {
                await _userManager.RemoveLoginAsync(waiter, login.LoginProvider, login.ProviderKey);
            }

            if (rolesForUser.Count > 0)
            {
                foreach (var item in rolesForUser)
                {
                    await _userManager.RemoveFromRoleAsync(waiter, item);
                }
            }

            var result = await _userManager.DeleteAsync(waiter);

            return result.Succeeded ? OperationResult.Succeeded : OperationResult.Failed;
        }

        public async Task<OperationResult> SetStatus(string email)
        {
            var waiterStatus = await _waiterRepository.GetWaiterStatus(email);

            if (waiterStatus != null)
            {
                waiterStatus.IsActive = false;
                _waiterRepository.Update(waiterStatus);
                await _unitOfWork.CommitChangesAsync();

                return OperationResult.Succeeded;
            }

            return OperationResult.Failed;
        }
    }
}
