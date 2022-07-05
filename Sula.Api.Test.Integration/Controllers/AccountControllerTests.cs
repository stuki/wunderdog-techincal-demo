using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Sula.Core.Models.Request;
using Sula.Core.Models.Response;
using Sula.Core.Services.Interfaces;
using Sula.Core.Test.Utils;
using Moq;
using Sula.Api;
using Xunit;

namespace Mokki.Api.Test.Integration.Controllers
{
    public class AccountControllerTests : IClassFixture<IntegrationTestBase<Startup>>
    {
        private readonly HttpClient _client;
        private const string BaseUrl = "api/v1/account";

        public AccountControllerTests(IntegrationTestBase<Startup> factory)
        {
            var emailServiceMock = new Mock<IEmailService>();
            
            _client = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddScoped(provider => emailServiceMock.Object);
                });
            }).CreateClient();
        }

        [Fact]
        public async Task Get_Returns_200AndUser_When_Successful()
        {
            await _client.GetToken(TestUsers.Default.Email, TestUsers.Password);

            var response = await _client.GetAndAssertSuccessAsync(BaseUrl);

            var user = await response.GetContentAsJsonAsync<UserResponse>();

            Assert.Equal(TestUsers.Default.Email, user.Email);
            Assert.Equal(TestUsers.Default.Settings.Temperature, user.Settings.Temperature);
            Assert.Equal(TestUsers.Default.Settings.Language, user.Settings.Language);
            Assert.Equal(TestUsers.Default.Sensors.Count(), user.Sensors.Count());
        }

        [Fact]
        public async Task Get_Returns_401_When_NotAuthorized()
        {
            var response = await _client.GetAsync(BaseUrl);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task ForgotPassword_Returns_200_When_Anonymous()
        {
            var request = new ForgotPasswordRequest
            {
                Email = TestUsers.Default.Email
            };

            await _client.PostAsJsonAndAssertSuccessAsync(BaseUrl + "/forgot", request);
        }

        [Fact]
        public async Task ForgotPassword_Returns_200_When_User_IsNotFound()
        {
            var request = new ForgotPasswordRequest
            {
                Email = "does.not@exist.com"
            };

            await _client.PostAsJsonAndAssertSuccessAsync(BaseUrl + "/forgot", request);
        }
    }
}