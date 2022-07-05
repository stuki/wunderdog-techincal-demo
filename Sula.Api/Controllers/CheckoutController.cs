using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sula.Core.Configurations;
using Sula.Core.Models.Stripe;
using Sula.Core.Services.Interfaces;
using Stripe;
using Stripe.Checkout;

namespace Sula.Api.Controllers
{
    [AllowAnonymous]
    [Route("api/v1/checkout")]
    public class CheckoutController : ControllerBase
    {
        private readonly IStripeClient _client;
        private readonly IOptions<StripeOptions> _options;
        private readonly ILogger<CheckoutController> _logger;
        private readonly IOrderService _orderService;

        public CheckoutController(IOptions<StripeOptions> options, ILogger<CheckoutController> logger, IOrderService orderService)
        {
            _logger = logger;
            _orderService = orderService;
            _options = options;
            _client = new StripeClient(_options.Value.SecretKey);
        }

        [HttpGet("setup")]
        public SetupResponse Setup()
        {
            return new SetupResponse
            {
                AirwitsPriceId = _options.Value.AirwitsPriceId,
                PublishableKey = _options.Value.PublishableKey,
                AirwitsPrice = _options.Value.AirwitsPrice,
                BasePrice = _options.Value.BasePrice
            };
        }

        [HttpPost]
        public async Task<IActionResult> CreateCheckoutSession([FromBody] CreateCheckoutSessionRequest request)
        {
            var options = new SessionCreateOptions
            {
                SuccessUrl = $"{_options.Value.Domain}/success?sessionId={{CHECKOUT_SESSION_ID}}",
                CancelUrl = $"{_options.Value.Domain}/cancel?sessionId={{CHECKOUT_SESSION_ID}}",
                PaymentMethodTypes = new List<string>
                {
                    "card"
                },
                Mode = "subscription",
                BillingAddressCollection = "auto",
                ShippingAddressCollection = new SessionShippingAddressCollectionOptions
                {
                    AllowedCountries = _options.Value.AllowedCountries.ToList()
                },
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        Price = _options.Value.BasePriceId,
                        Quantity = 1
                    },
                    new SessionLineItemOptions
                    {
                        Price = request.PriceId,
                        Quantity = request.Quantity
                    }
                }
            };

            var stripeSessionService = new SessionService(_client);
            
            try
            {
                var session = await stripeSessionService.CreateAsync(options);

                await _orderService.CreateOrderAsync(session.Id, request.Quantity);
                
                return Ok(new CreateCheckoutSessionResponse
                {
                    SessionId = session.Id
                });
            }
            catch (StripeException exception)
            {
                _logger.LogError(exception.StripeError.Message);
                return BadRequest(new ErrorResponse
                {
                    Error = new ErrorMessage
                    {
                        Message = exception.StripeError.Message
                    }
                });
            }
        }
    }
}
