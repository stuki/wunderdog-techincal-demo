using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Sula.Core.Test.Utils;
using OpenIddict.Abstractions;
using Sula.Api;
using Xunit;

namespace Mokki.Api.Test.Integration.Controllers
{
    public class AuthorizationControllerTests : IClassFixture<IntegrationTestBase<Startup>>
    {
        private readonly HttpClient _client;
        private const string BaseUrl = "/connect/token";

        public AuthorizationControllerTests(IntegrationTestBase<Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Exchange_Returns_Token_With_Correct_UsernameAndPassword()
        {
            var request = CreateAuthRequest();

            var response = await _client.SendAsync(request, HttpCompletionOption.ResponseContentRead);

            Assert.True(response.IsSuccessStatusCode);

            var payload = await response.Content.ReadFromJsonAsync<OpenIddictResponse>();

            Assert.Null(payload.Error);
            Assert.NotNull(payload.AccessToken);
            Assert.NotNull(payload.RefreshToken);
            Assert.NotNull(payload.IdToken);
        }

        [Fact]
        public async Task Exchange_Returns_400_With_Incorrect_UsernameAndPassword()
        {
            var request = CreateAuthRequest("not right");

            var response = await _client.SendAsync(request, HttpCompletionOption.ResponseContentRead);

            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var payload = await response.Content.ReadFromJsonAsync<OpenIddictResponse>();

            Assert.NotNull(payload.Error);
            Assert.Equal("invalid_grant", payload.Error);
            Assert.Null(payload.AccessToken);
            Assert.Null(payload.RefreshToken);
            Assert.Null(payload.IdToken);
        }

        [Fact]
        public async Task Exchange_Returns_Token_Using_RefreshToken()
        {
            var tokenResponse = await _client.SendAsync(CreateAuthRequest(), HttpCompletionOption.ResponseContentRead);
            
            var tokenResponsePayload = await tokenResponse.Content.ReadFromJsonAsync<OpenIddictResponse>();
            
            var request = CreateRefreshAuthRequest(tokenResponsePayload.RefreshToken);

            var response = await _client.SendAsync(request, HttpCompletionOption.ResponseContentRead);

            Assert.True(response.IsSuccessStatusCode);

            var payload = await response.Content.ReadFromJsonAsync<OpenIddictResponse>();

            Assert.Null(payload.Error);
            Assert.NotNull(payload.AccessToken);
            Assert.NotNull(payload.RefreshToken);
            Assert.NotNull(payload.IdToken);
        }

        private static HttpRequestMessage CreateAuthRequest(string password = TestUsers.Password)
        {
            return new HttpRequestMessage(HttpMethod.Post, BaseUrl)
            {
                Content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    ["grant_type"] = OpenIddictConstants.GrantTypes.Password, 
                    ["username"] = TestUsers.Default.Email,
                    ["password"] = password,
                    ["scope"] = "openid offline_access",
                    ["audience"] = "mobile",
                })
            };
        }
        
        private static HttpRequestMessage CreateRefreshAuthRequest(string refreshToken)
        {
            return new HttpRequestMessage(HttpMethod.Post, BaseUrl)
            {
                Content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    ["grant_type"] = OpenIddictConstants.GrantTypes.RefreshToken, 
                    ["refresh_token"] = refreshToken,
                    ["scope"] = "openid offline_access",
                    ["audience"] = "mobile",
                })
            };
        }
    }
}