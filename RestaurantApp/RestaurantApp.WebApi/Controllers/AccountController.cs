using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using RestaurantApp.WebApi.Entities;
using RestaurantApp.WebApi.Models;
using Serilog;

namespace RestaurantApp.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public AccountController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IConfiguration configuration,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _context = context;
        }

        [HttpPost]
        public async Task<object> Login([FromBody] LoginDto model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

            if (result.Succeeded)
            {
                Log.Information(string.Format("User: {0} has been logged in successfuly.", model.Email));
                var appUser = _userManager.Users.SingleOrDefault(r => r.Email == model.Email);
                return await GenerateJwtToken(model.Email, appUser);
            }

            return Unauthorized();
        }

        [HttpPost]
        public async Task<object> Register([FromBody] RegisterDto model)
        {
            var user = new IdentityUser
            {
                UserName = model.Email,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                await _userManager.AddToRoleAsync(user, "User");
                Log.Information(string.Format("User: {0} has been registered successfuly.", model.Email));

                var token = await GenerateJwtToken(model.Email, user);

                return Ok(token);
            }

            return BadRequest();
        }

        [HttpPost("{email}")]
        [Route("forgotMyPassword")]
        public async Task<ActionResult> ForgotMyPassword(string email)
        {
            try
            {
                var user = _userManager.Users.SingleOrDefault(u => u.Email == email);
                if (user != null)
                {
                    var code = GenerateCode();
                    var entity = new ForgotPassword()
                    {
                        Email = email,
                        Code = code,
                        CreatedAt = DateTime.Now,
                        CreatedBy = email,
                        LastUpdatedAt = DateTime.Now,
                        LastUpdatedBy = email
                    };

                    // save in database
                    var result = await _context.ForgotPassword.AddAsync(entity);

                    var message = new MimeMessage();
                    message.From.Add(new MailboxAddress("From", "alex.chiurtu@gmail.com"));
                    message.To.Add(new MailboxAddress("To", email));
                    message.Subject = "RestaurantApp: Code for reset password";
                    message.Body = new TextPart("html")
                    {
                        Text = "Code: " + code
                    };

                    using (var client = new SmtpClient())
                    {
                        client.Connect("smtp.gmail.com", 587, false);
                        try
                        {
                            client.Authenticate("alex.chiurtu@gmail.com", "Alexandru2005");
                        }
                        catch (Exception ex)
                        {
                            throw new ApplicationException(ex.ToString());

                        }
                        client.Send(message);
                        client.Disconnect(true);
                    }
                    return Ok();
                }

                return NoContent();
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.ToString());

            }
        }

        [HttpGet]
        [Route("verifyCode")]
        public async Task<ActionResult> VerifyCode(string code, string email)
        {
            try
            {
                var result = await _context.ForgotPassword.Where(x => x.Email == email && x.Code == code).OrderByDescending(x => x.CreatedAt).FirstOrDefaultAsync();

                if (result == null)
                {
                    return NoContent();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPut]
        [Route("updatePassword")]
        public async Task<ActionResult> UpdatePassword(PasswordsRequest model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = _userManager.Users.SingleOrDefault(r => r.Email == model.Email);
                    if (user != null)
                    {
                        

                        _userManager.Change

                        appUser.PasswordHash = HashPassword(model.NewPassword);
                        var result = await userManager.UpdateAsync(appUser);
                        return Ok(result);
                    }
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<object> GenerateJwtToken(string email, IdentityUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Token");

            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("Waiter"))
               await SetWaiterStatus(email);

            claims.AddRange(roles.Select(role => new Claim("Role", role)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["JwtExpireDays"]));

            var token = new JwtSecurityToken(
                _configuration["JwtIssuer"],
                _configuration["JwtIssuer"],
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public class LoginDto
        {
            [Required]
            public string Email { get; set; }

            [Required]
            public string Password { get; set; }
        }

        public class RegisterDto
        {
            [Required]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "PASSWORD_MIN_LENGTH", MinimumLength = 6)]
            public string Password { get; set; }
        }

        private async Task SetWaiterStatus(string email)
        {
            var result = await _context.WaitersStatus.FindAsync(email);

            if (result == null)
            {
                WaiterStatus status = new WaiterStatus()
                {
                    Waiter = email,
                    IsActive = true
                };

                await _context.WaitersStatus.AddAsync(status);
                await _context.SaveChangesAsync();
            }
            else
            {
                result.IsActive = true;
                _context.Entry(result).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
            }
        }

        private string GenerateCode()
        {
            Random random = new Random();
            const string pool = "abcdefghijklmnopqrstuvwxyz0123456789";
            var builder = new StringBuilder();

            for (var i = 0; i < 6; i++)
            {
                var c = pool[random.Next(0, pool.Length)];
                builder.Append(c);
            }

            return builder.ToString();
        }
    }
}
