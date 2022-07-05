using System.Threading.Tasks;
using Sula.Core.Models;
using Sula.Core.Models.Support;

namespace Sula.Core.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendUserConfirmationMessageAsync(string email, ConfirmationEmailTemplateOptions options);
        Task SendUserWelcomeMessageAsync(string email);
        Task SendResetToken(string email, string token);
    }
}