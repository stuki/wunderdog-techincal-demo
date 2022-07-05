using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sula.Core.Configurations;
using Stripe;
using Stripe.Checkout;

namespace Sula.Api.Controllers
{
    [Route("api/v1/webhook")]
    public class StripeWebhookController : ControllerBase
    {
        private readonly IOptions<StripeOptions> _options;
        private readonly ILogger<StripeWebhookController> _logger;

        public StripeWebhookController(IOptions<StripeOptions> options, ILogger<StripeWebhookController> logger)
        {
            _options = options;
            _logger = logger;
        }

        [HttpPost("stripe")]
        public async Task<IActionResult> Webhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            Event stripeEvent;
            try
            {
                stripeEvent = EventUtility.ConstructEvent(
                    json,
                    Request.Headers["Stripe-Signature"],
                    _options.Value.WebhookSecret
                );
                _logger.LogInformation($"Webhook notification with type: {stripeEvent.Type} found for {stripeEvent.Id}");
            }
            catch (Exception exception)
            {
                _logger.LogError($"Something failed {exception}");
                return BadRequest();
            }

            if (stripeEvent.Type == "checkout.session.completed")
            {
                var session = stripeEvent.Data.Object as Session;
                _logger.LogInformation($"Session ID: {session.Id}");
                // Take some action based on session.
            }

            return Ok();
        }
    }
}