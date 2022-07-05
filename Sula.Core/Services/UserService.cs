using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Sula.Core.Models;
using Sula.Core.Exceptions;
using Sula.Core.Models.Entity;
using Sula.Core.Models.Request;
using Sula.Core.Services.Interfaces;

namespace Sula.Core.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;

        public UserService(UserManager<ApplicationUser> userManager, IEmailService emailService)
        {
            _userManager = userManager;
            _emailService = emailService;
        }

        public async Task<ApplicationUser> GetUserAsync(ClaimsPrincipal claimsPrincipal)
        {
            return await _userManager.FindByNameAsync(claimsPrincipal.Identity.Name);
        }

        public async Task<ApplicationUser> GetUserAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<IdentityResult> UpdateAsync(ApplicationUser user)
        {
            return await _userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> CreateUserAsync(ApplicationUser user)
        {
            return await _userManager.CreateAsync(user);
        }

        public async Task<string> GetConfirmationTokenAsync(ApplicationUser user)
        {
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public async Task<string> GetPhoneConfirmationTokenAsync(ApplicationUser user, string phoneNumber)
        {
            return await _userManager.GenerateChangePhoneNumberTokenAsync(user, phoneNumber);
        }

        public async Task<bool> ConfirmEmailAsync(ApplicationUser user, string confirmationToken)
        {
            var result = await _userManager.ConfirmEmailAsync(user, confirmationToken);

            if (!result.Succeeded)
            {
                return false;
            }

            await _emailService.SendUserWelcomeMessageAsync(user.Email);

            return true;
        }

        public async Task<bool> ConfirmPhoneAsync(ApplicationUser user, PhoneConfirmationRequest request)
        {
            return await _userManager.VerifyChangePhoneNumberTokenAsync(user, request.ConfirmationToken,
                request.PhoneNumber);
        }

        public async Task SendPasswordResetToken(string requestEmail)
        {
            var user = await _userManager.FindByEmailAsync(requestEmail);

            if (user == null)
            {
                throw new UserNotFoundException();
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            await _emailService.SendResetToken(user.Email, token);
        }

        public async Task<bool> AddPasswordAsync(string userId, string password)
        {
            var user = await _userManager.FindByIdAsync(userId);

            var result = await _userManager.AddPasswordAsync(user, password);

            return result.Succeeded;
        }

        public async Task<bool> VerifyResetToken(string token, string userId, string password)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new UserNotFoundException();
            }

            var result = await _userManager.ResetPasswordAsync(user, token, password);

            return result.Succeeded;
        }
    }
}