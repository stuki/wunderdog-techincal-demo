using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Sula.Core.Models;
using Sula.Core.Models.Entity;
using Sula.Core.Models.Request;

namespace Sula.Core.Services.Interfaces
{
    public interface IUserService
    {
        Task<ApplicationUser> GetUserAsync(ClaimsPrincipal claimsPrincipal);
        Task<ApplicationUser> GetUserAsync(string email);
        Task<IdentityResult> UpdateAsync(ApplicationUser user);
        Task<IdentityResult> CreateUserAsync(ApplicationUser user);
        Task<string> GetConfirmationTokenAsync(ApplicationUser user);
        Task<string> GetPhoneConfirmationTokenAsync(ApplicationUser user, string phoneNumber);
        Task<bool> ConfirmEmailAsync(ApplicationUser user, string confirmationToken);
        Task<bool> ConfirmPhoneAsync(ApplicationUser user, PhoneConfirmationRequest request);
        Task<bool> VerifyResetToken(string userId, string token, string password);
        Task SendPasswordResetToken(string requestEmail);
        Task<bool> AddPasswordAsync(string userId, string password);
    }
}