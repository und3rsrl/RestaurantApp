using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using RestaurantApp.BusinessEntities.DTOs.Account;
using RestaurantApp.BusinessService.Interfaces;
using RestaurantApp.Common.Enums;
using RestaurantApp.DataContracts.Interfaces;
using RestaurantApp.DataModel.Models;
using RestaurantApp.DataServices.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantApp.BusinessService.Implementation
{
    public class AccountBusinessService : IAccountBusinessService
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IForgotPasswordRepository _forgotPasswordRepository;
        private readonly IWaiterRepository _waiterRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public AccountBusinessService
            (
                UserManager<IdentityUser> userManager,
                SignInManager<IdentityUser> signInManager,
                IForgotPasswordRepository forgotPasswordRepository,
                IConfiguration configuration,
                IWaiterRepository waiterRepository
            )
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = configuration;
            _forgotPasswordRepository = forgotPasswordRepository;
            _waiterRepository = waiterRepository;
        }

        public async Task<string> Login(LoginDetails loginDetails)
        {
            var authentication = await _signInManager.PasswordSignInAsync(loginDetails.Email, loginDetails.Password, false, false);

            if (authentication.Succeeded)
            {
                var user = _userManager.Users.SingleOrDefault(r => r.Email == loginDetails.Email);
                return await GenerateJwtToken(loginDetails.Email, user);
            }

            return string.Empty;
        }

        public async Task<string> Register(RegisterDetails registerDetails)
        {
            var newUser = new IdentityUser
            {
                UserName = registerDetails.Email,
                Email = registerDetails.Email,
            };

            var creationResult = await _userManager.CreateAsync(newUser, registerDetails.Password);

            if (creationResult.Succeeded)
            {
                await _signInManager.SignInAsync(newUser, false);
                await _userManager.AddToRoleAsync(newUser, "User");

                return await GenerateJwtToken(registerDetails.Email, newUser);
            }

            return string.Empty;
        }

        public async Task<OperationResult> PasswordRecovery(string email)
        {
            try
            {
                var user = _userManager.Users.SingleOrDefault(u => u.Email == email);
                if (user != null)
                {
                    var code = GenerateCode();

                    var recoveryEntry = new ForgotPassword
                    {
                        Email = email,
                        Code = code,
                        CreatedAt = DateTime.Now,
                        CreatedBy = email,
                        LastUpdatedAt = DateTime.Now,
                        LastUpdatedBy = email
                    };

                    _forgotPasswordRepository.Add(recoveryEntry);
                    await _unitOfWork.CommitChangesAsync();

                    var emailTemplate = GenerateVerificationEmail(email, code);
                    await SendVerificationEmail(emailTemplate);

                    return OperationResult.Succeeded;
                }

                return OperationResult.Failed;
            }
            catch (Exception)
            {
                return OperationResult.Failed;
            }
        }

        public async Task<OperationResult> ValidateVerificationCode(string verificationCode, string email)
        {
            var latestForgotPasswordEntry = await _forgotPasswordRepository.GetLatestForgotPasswordEntry(email, verificationCode);

            if (latestForgotPasswordEntry != null)
            {
                return OperationResult.Succeeded;
            }

            return OperationResult.Failed;
        }

        public async Task<OperationResult> UpdatePassword(PasswordChangeRequest passwordChangeRequest)
        {
            var user = _userManager.Users.SingleOrDefault(u => u.Email == passwordChangeRequest.Email);
            if (user != null)
            {
                user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, passwordChangeRequest.Password);
                await _userManager.UpdateAsync(user);

                return OperationResult.Succeeded;
            }

            return OperationResult.Failed;
        }

        private async Task<string> GenerateJwtToken(string email, IdentityUser user)
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
            {
                await SetWaiterStatus(email);
            }
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

        private string GenerateCode()
        {
            var randomGenerator = new Random();
            var charactersToPickFrom = "abcdefghijklmnopqrstuvwxyz0123456789";
            var verifyCodeBuilder = new StringBuilder();

            for (var i = 0; i < 6; i++)
            {
                verifyCodeBuilder.Append(randomGenerator.Next(0, charactersToPickFrom.Length));
            }

            return verifyCodeBuilder.ToString();
        }

        private MimeMessage GenerateVerificationEmail(string email, string verificationCode)
        {
            var emailTemplate = new MimeMessage();
            emailTemplate.From.Add(new MailboxAddress("From", "alex.chiurtu@gmail.com"));
            emailTemplate.To.Add(new MailboxAddress("To", email));
            emailTemplate.Subject = "RestaurantApp: Code for reset password";
            emailTemplate.Body = new TextPart("html")
            {
                Text = "Code: " + verificationCode
            };

            return emailTemplate;
        }

        private async Task SendVerificationEmail(MimeMessage emailTemplate)
        {
            using (var smtpClient = new SmtpClient())
            {
                await smtpClient.ConnectAsync("smtp.gmail.com", 587, false);
                await smtpClient.AuthenticateAsync("alex.chiurtu@gmail.com", "Alexandru2005");
                await smtpClient.SendAsync(emailTemplate);
                await smtpClient.DisconnectAsync(true);
            }
        }

        private async Task SetWaiterStatus(string email)
        {
            var result = await _waiterRepository.GetWaiterStatus(email);

            if (result == null)
            {
                var status = new WaiterStatus
                {
                    Waiter = email,
                    IsActive = true
                };
                _waiterRepository.Add(status);

            }
            else
            {
                result.IsActive = true;
                _waiterRepository.Update(result);

            }

            await _unitOfWork.CommitChangesAsync();
        }
    }
}
