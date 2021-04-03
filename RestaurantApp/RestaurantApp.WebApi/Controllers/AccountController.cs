using Microsoft.AspNetCore.Mvc;
using RestaurantApp.BusinessEntities.DTOs.Account;
using RestaurantApp.BusinessService.Interfaces;
using System.Threading.Tasks;

namespace RestaurantApp.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountBusinessService _accountBusinessService;

        public AccountController
            (
                IAccountBusinessService accountBusinessService
            )
        {
            _accountBusinessService = accountBusinessService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login([FromBody] LoginDetails loginDetails)
        {
            var token = await _accountBusinessService.Login(loginDetails);

            if (!string.IsNullOrEmpty(token))
            {
                return Ok(token);
            }

            return Unauthorized();
        }

        [HttpPost("register")]
        public async Task<ActionResult<string>> Register([FromBody] RegisterDetails registerDetails)
        {
            var token = await _accountBusinessService.Register(registerDetails);

            if (!string.IsNullOrEmpty(token))
            {
                return Ok(token);
            }

            return BadRequest();
        }

        [HttpPost("forgotMyPassword/{email}")]
        public async Task<ActionResult> ForgotMyPassword(string email)
        {
            var result = await _accountBusinessService.PasswordRecovery(email);

            if (result == Common.Enums.OperationResult.Succeeded)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpGet("verifyCode/{code}&{email}")]
        public async Task<ActionResult> VerifyCode(string code, string email)
        {
            var result = await _accountBusinessService.ValidateVerificationCode(code, email);

            if (result == Common.Enums.OperationResult.Succeeded)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpPut]
        public async Task<ActionResult> UpdatePassword([FromBody] PasswordChangeRequest passwordChangeDetails)
        {
            var result = await _accountBusinessService.UpdatePassword(passwordChangeDetails);

            if (result == Common.Enums.OperationResult.Succeeded)
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}
