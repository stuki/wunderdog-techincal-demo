using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Sula.Core.Models;
using Sula.Core.Models.Request;
using Sula.Core.Services.Interfaces;
using Sula.Core.Test.Utils;
using Moq;
using Sula.Api;
using Xunit;

namespace Mokki.Api.Test.Integration.Controllers
{
    public class PhoneControllerTests : IClassFixture<IntegrationTestBase<Startup>>
    {
        private readonly HttpClient _client;
        private const string BaseUrl = "/api/v1/phone";
        private readonly Mock<ITextMessageService> _textMessageServiceMock;

        public PhoneControllerTests(IntegrationTestBase<Startup> factory)
        {
            _textMessageServiceMock = new Mock<ITextMessageService>();

            _client = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddScoped(provider => _textMessageServiceMock.Object);
                });
            }).CreateClient();
        }

        [Fact]
        public async Task SendConfirmationMessage_Returns_200_When_Successful()
        {
            await _client.GetToken(TestUsers.Default.Email, TestUsers.Password);

            var request = new PhoneRequest
            {
                PhoneNumber = TestUsers.Default.PhoneNumber
            };

            await _client.PostAsJsonAndAssertSuccessAsync(BaseUrl + "/send", request);
        }

        [Fact]
        public async Task ConfirmPhoneNumber_Returns_200_When_Successful()
        {
            await _client.GetToken(TestUsers.Default.Email, TestUsers.Password);

            await _client.PostAsJsonAndAssertSuccessAsync(BaseUrl + "/send", new PhoneRequest
            {
                PhoneNumber = TestUsers.Default.PhoneNumber
            });

            var token = _textMessageServiceMock.Invocations.First().Arguments.First().ToString();

            var request = new PhoneConfirmationRequest
            {
                PhoneNumber = TestUsers.Default.PhoneNumber,
                ConfirmationToken = token
            };

            await _client.PostAsJsonAndAssertSuccessAsync(BaseUrl + "/confirm", request);
        }

        [Fact]
        public async Task SendConfirmationMessage_Returns_401_When_Not_Authenticated()
        {
            var request = new PhoneRequest
            {
                PhoneNumber = TestUsers.Default.PhoneNumber
            };

            var response = await _client.PostAsJsonAsync(BaseUrl + "/send", request);
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task ConfirmPhoneNumber_Returns_401_When_Not_Authenticated()
        {
            var request = new PhoneConfirmationRequest
            {
                PhoneNumber = TestUsers.Default.PhoneNumber,
                ConfirmationToken = "123456"
            };

            var response = await _client.PostAsJsonAsync(BaseUrl + "/confirm", request);
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}