using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using OpenIddict.Abstractions;
using Xunit;

namespace Sula.Core.Test.Utils
{
    public static class HttpClientExtensions
    {
        public static async Task<HttpResponseMessage> GetAndAssertSuccessAsync(this HttpClient httpClient,
            string requestUri)
        {
            var response = await httpClient.GetAsync(requestUri);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            return response;
        }

        public static async Task<HttpResponseMessage> PostAsJsonAndAssertSuccessAsync<T>(this HttpClient httpClient,
            string requestUri, T value)
        {
            var response = await httpClient.PostAsJsonAsync(requestUri, value);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            return response;
        }

        public static async Task<HttpResponseMessage> PutAsJsonAndAssertSuccessAsync<T>(this HttpClient httpClient,
            string requestUri, T value)
        {
            var response = await httpClient.PutAsJsonAsync(requestUri, value);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            return response;
        }

        public static async Task<HttpResponseMessage> DeleteAndAssertSuccessAsync<T>(this HttpClient httpClient,
            string requestUri)
        {
            var response = await httpClient.DeleteAsync(requestUri);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            return response;
        }

        public static async Task<T> GetContentAsJsonAsync<T>(this HttpResponseMessage response)
        {
            return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
        }

        public static async Task GetToken(this HttpClient client, string userName, string password)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "/connect/token")
            {
                Content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    ["grant_type"] = "password", ["username"] = userName, ["password"] = password
                })
            };

            var response = await client.SendAsync(request, HttpCompletionOption.ResponseContentRead);

            var payload = await response.Content.ReadFromJsonAsync<OpenIddictResponse>();
            
            Assert.Null(payload.Error);
            Assert.NotNull(payload.AccessToken);

            var token = payload.AccessToken;

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }
}