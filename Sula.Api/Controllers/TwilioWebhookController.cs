using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sula.Core.Configurations;
using Sula.Core.Models;
using Sula.Core.Models.Support;
using Sula.Core.Platform;
using Twilio.Security;

namespace Sula.Api.Controllers
{
    [Route("api/v1/webhook")]
    [AllowAnonymous]
    public class TwilioWebhookController : ControllerBase
    {
        private readonly DatabaseContext _databaseContext;
        private readonly ILogger<TwilioWebhookController> _logger;
        private readonly IOptions<TwilioOptions> _options;

        public TwilioWebhookController(DatabaseContext databaseContext, ILogger<TwilioWebhookController> logger,
            IOptions<TwilioOptions> options)
        {
            _databaseContext = databaseContext;
            _logger = logger;
            _options = options;
        }

        [HttpPost("twilio")]
        public async Task<IActionResult> Webhook([FromForm] string messageStatus, [FromForm] string messageSid, [FromForm] int? errorCode)
        {
            if (!IsValidRequest(Request))
            {
                return BadRequest();
            }
            
            try
            {
                var message = await _databaseContext.TextMessages.FindAsync(messageSid);

                if (message == null)
                {
                    throw new Exception("No message found with id: " + messageSid);
                }

                message.Status = StatusToEnum(messageStatus);
                message.ErrorCode = errorCode;
                message.UpdatedAt = DateTimeOffset.Now;

                await _databaseContext.SaveChangesAsync();

                return Ok();
            }
            catch (Exception exception)
            {
                _logger.LogWarning(exception, "Twilio webhook failed to update logs");
                return BadRequest(exception.Message);
            }
        }
        
        private bool IsValidRequest(HttpRequest request)
        {
            var requestValidator = new RequestValidator(_options.Value.Password);
            
            var signature = request.Headers["X-Twilio-Signature"];
            
            var requestUrl = $"{request.Scheme}://{request.Host}{request.Path}{request.QueryString}";
            
            var parameters = request.Form.Keys
                .Select(key => new {Key = key, Value = request.Form[key]})
                .ToDictionary(p => p.Key, p => p.Value.ToString());
            
            return requestValidator.Validate(requestUrl, parameters, signature);
        }

        private static MessageStatus StatusToEnum(string status)
        {
            return status switch
            {
                "failed" => MessageStatus.Failed,
                "sent" => MessageStatus.Sent,
                "delivered" => MessageStatus.Delivered,
                "undelivered" => MessageStatus.Undelivered,
                "queued" => MessageStatus.Queued,
                _ => MessageStatus.None
            };
        }
    }
}