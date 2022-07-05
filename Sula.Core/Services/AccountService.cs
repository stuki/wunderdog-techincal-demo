using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.EntityFrameworkCore;
using Sula.Core.Exceptions;
using Sula.Core.Models.Entity;
using Sula.Core.Models.Request;
using Sula.Core.Models.Support;
using Sula.Core.Platform;
using Sula.Core.Services.Interfaces;

namespace Sula.Core.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUserService _userService;
        private readonly DatabaseContext _databaseContext;
        private readonly IEmailService _emailService;

        public AccountService(IUserService userService, IEmailService emailService, DatabaseContext databaseContext)
        {
            _userService = userService;
            _emailService = emailService;
            _databaseContext = databaseContext;
        }

        public async Task<ApplicationUser> GetUserAsync(ClaimsPrincipal claimsPrincipal)
        {
            var user = await _userService.GetUserAsync(claimsPrincipal);
            
            if (user == null)
            {
                throw new UserNotFoundException();
            }
            
            await _databaseContext
                .Entry(user)
                .Collection(u => u.Sensors)
                .Query()
                .Include(sensor => sensor.Limits)
                .LoadAsync();
            await _databaseContext.Entry(user).Reference(u => u.Settings).LoadAsync();
            await _databaseContext.Entry(user).Reference(u => u.Address).LoadAsync();

            return user;
        }

        public async Task CreateUserAsync(ApplicationUser user, string baseUrl)
        {
            try
            {
                var userCheck = await _userService.GetUserAsync(user.Email);

                if (userCheck != null)
                {
                    throw new UserExistsException();
                }

                await _userService.CreateUserAsync(user);

                var confirmationToken = await _userService.GetConfirmationTokenAsync(user);

                var url = baseUrl + "/confirm?email=" + HttpUtility.UrlEncode(user.Email) + "&confirmationToken=" +
                          HttpUtility.UrlEncode(confirmationToken);

                var options = new ConfirmationEmailTemplateOptions
                {
                    ConfirmationUrl = url
                };

                await _emailService.SendUserConfirmationMessageAsync(user.Email, options);
            }
            catch (Exception exception)
            {
                throw new RegistrationException(ErrorCode.ErrorCreatingUser, exception.Message, exception);
            }

            await _databaseContext.SaveChangesAsync();
        }

        public async Task<string> GetPhoneNumberConfirmationTokenAsync(ApplicationUser user, PhoneRequest request)
        {
            user.PhoneNumber = null;
            user.PhoneNumberConfirmed = false;

            await _userService.UpdateAsync(user);

            return await _userService.GetPhoneConfirmationTokenAsync(user, request.PhoneNumber);
        }

        public async Task ConfirmPhoneNumberAsync(ApplicationUser user, PhoneConfirmationRequest request)
        {
            if (await _userService.ConfirmPhoneAsync(user, request))
            {
                user.PhoneNumber = request.PhoneNumber;
                user.PhoneNumberConfirmed = true;

                await _userService.UpdateAsync(user);
            }
            else
            {
                throw new CodeMismatchException();
            }
        }
    }
}