using System.Net.Http;
using Sula.Api;
using Sula.Core.Test.Utils;
using Xunit;

namespace Mokki.Api.Test.Integration.Controllers
{
    public class StripeWebhookControllerTests : IClassFixture<IntegrationTestBase<Startup>>
    {
        private readonly IntegrationTestBase<Startup> _factory;
        private readonly HttpClient _client;

        public StripeWebhookControllerTests(IntegrationTestBase<Startup> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }
    }
}