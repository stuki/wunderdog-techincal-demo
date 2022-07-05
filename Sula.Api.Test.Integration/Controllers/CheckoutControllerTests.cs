using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Sula.Core.Models.Stripe;
using Sula.Core.Test.Utils;
using Stripe.BillingPortal;
using Sula.Api;
using Xunit;

namespace Mokki.Api.Test.Integration.Controllers
{
    public class CheckoutControllerTests : IClassFixture<IntegrationTestBase<Startup>>
    {
        private readonly HttpClient _client;
        private const string BaseUrl = "/api/v1/checkout";

        public CheckoutControllerTests(IntegrationTestBase<Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Setup_Returns_200_When_Successful()
        {
            var configurationSection = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json")
                .Build()
                .GetSection("Stripe");

            await _client.GetToken(TestUsers.Default.Email, TestUsers.Password);

            var response = await _client.GetAndAssertSuccessAsync(BaseUrl + "/setup");

            var setupResponse = await response.GetContentAsJsonAsync<SetupResponse>();

            Assert.NotNull(setupResponse);
            Assert.Equal(configurationSection["AirwitsPriceId"], setupResponse.AirwitsPriceId);
            Assert.Equal(configurationSection["PublishableKey"], setupResponse.PublishableKey);
            Assert.Equal(int.Parse(configurationSection["AirwitsPrice"]), setupResponse.AirwitsPrice);
            Assert.Equal(double.Parse(configurationSection["BasePrice"]), setupResponse.BasePrice);
        }
    }
}