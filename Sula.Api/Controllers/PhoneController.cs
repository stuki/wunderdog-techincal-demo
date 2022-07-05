using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sula.Core.Exceptions;
using Sula.Core.Models;
using Sula.Core.Models.Request;
using Sula.Core.Services.Interfaces;
using Twilio.Exceptions;

namespace Sula.Api.Controllers
{
    [Route("api/v1/phone")]
    public class PhoneController : ApiBase
    {
        private readonly ILogger<PhoneController> _logger;
        private readonly ITextMessageService _textMessageService;

        public PhoneController(
            IAccountService accountService,
            ITextMessageService textMessageService,
            ILogger<PhoneController> logger) : base(accountService)
        {
            _logger = logger;
            _textMessageService = textMessageService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendConfirmationMessage(PhoneRequest request)
        {
            try
            {
                var token = await AccountService.GetPhoneNumberConfirmationTokenAsync(await CurrentUser, request);
                await _textMessageService.SendConfirmationMessageAsync(token, request);    

                return Ok();
            }
            catch (ApiException exception)
            {
                _logger.LogWarning(exception, "Failed to send text message to user");
                return BadRequest();
            }
        }

        [HttpPost("confirm")]
        public async Task<IActionResult> ConfirmPhoneNumber(PhoneConfirmationRequest request)
        {
            try
            {
                await AccountService.ConfirmPhoneNumberAsync(await CurrentUser, request);
                return Ok();
            }
            catch (CodeMismatchException exception)
            {
                _logger.LogWarning(exception, exception.Code);
                return BadRequest(exception.Code);
            }
        }
    }
}