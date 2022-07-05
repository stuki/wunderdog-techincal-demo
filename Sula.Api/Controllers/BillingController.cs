using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sula.Core.Configurations;
using Sula.Core.Services.Interfaces;
using Stripe;
using Stripe.BillingPortal;

namespace Sula.Api.Controllers
{
    [Route("api/v1/billing")]
    public class BillingController : ApiBase
    {
        private readonly IStripeClient _client;
        private readonly IOptions<StripeOptions> _options;
        private readonly ILogger<BillingController> _logger;

        public BillingController(IOptions<StripeOptions> options, IAccountService accountService, ILogger<BillingController> logger) : base(accountService)
        {
            _logger = logger;
            _options = options;
            _client = new StripeClient(_options.Value.SecretKey);
        }
        
        [HttpGet]
        public async Task<IActionResult> GetCustomerPortal()
        {
            var user = await CurrentUser;

            var options = new SessionCreateOptions
            {
                Customer = user.StripeCustomerId,
                ReturnUrl = _options.Value.Domain,
            };
            
            var service = new SessionService(_client);
            var session = await service.CreateAsync(options);

            return Ok(session);
        }
    }
}