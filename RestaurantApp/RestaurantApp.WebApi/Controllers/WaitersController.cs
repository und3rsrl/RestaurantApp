using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RestaurantApp.WebApi.DTOs;
using static RestaurantApp.WebApi.Controllers.AccountController;

namespace RestaurantApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WaitersController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;

        public WaitersController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IEnumerable<WaiterDTO>> GetWaiters()
        {
            var waitersIdentity = await _userManager.GetUsersInRoleAsync("Waiter");

            List<WaiterDTO> waiters = new List<WaiterDTO>();

            foreach (var waiter in waitersIdentity)
            {
                waiters.Add(new WaiterDTO()
                {
                    UserId = waiter.Id,
                    Email = waiter.Email
                });
            }

            return waiters;
        }

        [HttpPost]
        public async Task<object> AddWaiter([FromBody] RegisterDto model)
        {
            var user = new IdentityUser
            {
                UserName = model.Email,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Waiter");

                return Ok();
            }

            return BadRequest("Account was not created.");
        }

        [HttpDelete("{id}")]
        public async Task<object> DeleteWaiter([FromRoute] string id)
        {
            if (id == null)
            {
                return BadRequest("Invalid user.");
            }

            var user = await _userManager.FindByIdAsync(id);

            var logins = await _userManager.GetLoginsAsync(user);

            var rolesForUser = await _userManager.GetRolesAsync(user);

            foreach (var login in logins)
            {
                await _userManager.RemoveLoginAsync(user, login.LoginProvider, login.ProviderKey);
            }

            if (rolesForUser.Count > 0)
            {
                foreach (var item in rolesForUser)
                {
                    await _userManager.RemoveFromRoleAsync(user, item);
                }
            }

            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
                return Ok("Account deleted successfully.");

            return BadRequest("Account was not deleted.");
        }
    }
}