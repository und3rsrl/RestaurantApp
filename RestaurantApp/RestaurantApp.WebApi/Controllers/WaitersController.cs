using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantApp.DataServices;
using RestaurantApp.WebApi.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using static RestaurantApp.WebApi.Controllers.AccountController;

namespace RestaurantApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WaitersController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly Entities _context;

        public WaitersController(
            UserManager<IdentityUser> userManager,
            Entities context)
        {
            _userManager = userManager;
            _context = context;
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

        [HttpPost]
        [Route("setStatus")]
        public async Task<object> SetStatus([FromBody] string email)
        {
            var result = await _context.WaitersStatus.FindAsync(email);

            if (result != null)
            {
                result.IsActive = false;
                _context.Entry(result).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }

                return Ok();
            }

            return BadRequest("Status was not set.");
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