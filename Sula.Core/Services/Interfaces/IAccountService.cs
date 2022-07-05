using System.Security.Claims;
using System.Threading.Tasks;
using Sula.Core.Models;
using Sula.Core.Models.Entity;
using Sula.Core.Models.Request;

namespace Sula.Core.Services.Interfaces
{
    public interface IAccountService
    {
        Task<ApplicationUser> GetUserAsync(ClaimsPrincipal claimsPrincipal);
        Task<string> GetPhoneNumberConfirmationTokenAsync(ApplicationUser user, PhoneRequest request);
        Task ConfirmPhoneNumberAsync(ApplicationUser user, PhoneConfirmationRequest request);
        Task CreateUserAsync(ApplicationUser user, string baseUrl);
    }
}