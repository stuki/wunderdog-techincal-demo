using System;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SendGrid;
using SendGrid.Helpers.Mail;
using Sula.Core.Configurations;
using Sula.Core.Exceptions;
using Sula.Core.Models.Support;
using Sula.Core.Services.Interfaces;

namespace Sula.Core.Services
{
    public class EmailService : IEmailService
    {
        private readonly SendGridClient _client;
        private readonly IOptions<SendGridOptions> _options;

        public EmailService(IOptions<SendGridOptions> options)
        {
            _options = options;
            _client = new SendGridClient(options.Value.ApiKey);
        }

        public async Task SendUserConfirmationMessageAsync(string email, ConfirmationEmailTemplateOptions options)
        {
            var template = _options.Value.Templates["Registration"];

            if (template == null)
            {
                throw new Exception();
            }

            await SendEmail(email, template, options);
        }

        public async Task SendUserWelcomeMessageAsync(string email)
        {
            var template = _options.Value.Templates["Welcome"];

            if (template == null)
            {
                throw new Exception();
            }
            
            await SendEmail(email, template, null);
        }

        public async Task SendResetToken(string email, string token)
        {
            var template = _options.Value.Templates["Reset"];

            if (template == null)
            {
                throw new Exception();
            }

            dynamic options = new ExpandoObject();
            options.token = token;

            await SendEmail(email, template, options);
        }

        private async Task SendEmail(string email, string template, object options)
        {
            var mailMessage = MailHelper.CreateSingleTemplateEmail(
                new EmailAddress(_options.Value.FromEmail, _options.Value.FromName),
                new EmailAddress(email),
                template,
                options
            );

            var response = await _client.SendEmailAsync(mailMessage);

            if (response.StatusCode != HttpStatusCode.Accepted)
            {
                var anonymousObject = new { errors = new[] { new { message = "" } } };
                var body = await response.Body.ReadAsStringAsync();
                var error = JsonConvert.DeserializeAnonymousType(body, anonymousObject);
                throw new EmailException(error.errors.First().message);
            }
        }
    }
}