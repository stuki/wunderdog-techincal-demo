using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Sula.Core.Test.Utils;
using Stripe.BillingPortal;
using Sula.Api;
using Xunit;

namespace Mokki.Api.Test.Integration.Controllers
{
    public class BillingControllerTests : IClassFixture<IntegrationTestBase<Startup>>
    {
        private readonly IntegrationTestBase<Startup> _factory;
        private readonly HttpClient _client;
        private const string BaseUrl = "/api/v1/billing";

        public BillingControllerTests(IntegrationTestBase<Startup> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }
        
        [Fact(Skip = "needs more work")]
        public async Task GetCustomerPortal_Returns_200_When_Successful()
        {
            await _client.GetToken(TestUsers.Default.Email, TestUsers.Password);
            
            var response = await _client.GetAndAssertSuccessAsync(BaseUrl);

            var session = await response.GetContentAsJsonAsync<Session>();
            
            Assert.NotNull(session);
            Assert.NotNull(session.Url);
        }
        
        [Fact]
        public async Task GetCustomerPortal_Returns_401_When_NotAuthorized()
        {
            var response = await _client.GetAsync(BaseUrl);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}