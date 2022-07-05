using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Sula.Core.Models;
using Sula.Core.Models.Entity;
using Sula.Core.Test.Utils;
using OpenIddict.Abstractions;
using Sula.Api;
using Xunit;

namespace Mokki.Api.Test.Integration.Controllers
{
    public class TwilioWebhookControllerTests : IClassFixture<IntegrationTestBase<Startup>>
    {
        private readonly IntegrationTestBase<Startup> _factory;
        private readonly HttpClient _client;
        private const string BaseUrl = "/api/v1/webhook/twilio";

        // Request content pulled from https://www.twilio.com/docs/usage/webhooks/sms-webhooks
        private readonly Dictionary<string, string> _content = new Dictionary<string, string>
        {
            ["SmsSid"] = "SM2xxxxxx",
            ["SmsStatus"] = "sent",
            ["Body"] = "McAvoy or Stewart? These timelines can get so confusing.",
            ["MessageStatus"] = "sent",
            ["To"] = "+1512zzzyyyy",
            ["MessageSid"] = "SM2xxxxxx",
            ["AccountSid"] = "ACxxxxxxx",
            ["From"] = "+1512xxxyyyy",
            ["ApiVersion"] = "2010-04-01"
        };

        public TwilioWebhookControllerTests(IntegrationTestBase<Startup> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
            var databaseContext = _factory.CreateDatabaseContext();

            if (databaseContext.TextMessages.FirstOrDefault() == null)
            {
                databaseContext.TextMessages.Add(new TextMessage {Id = "SM2xxxxxx"});
                databaseContext.SaveChanges();
            }
        }

        [Fact]
        public async Task Webhook_Returns_200_When_Successful()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, BaseUrl)
            {
                Content = new FormUrlEncodedContent(_content)
            };

            request.Headers.Add("X-Twilio-Signature", CreateValidationHash("http://localhost" + BaseUrl, _content));

            var response = await _client.SendAsync(request);

            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task Webhook_Returns_400_When_Signature_Is_NotValid()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, BaseUrl)
            {
                Content = new FormUrlEncodedContent(_content)
            };

            request.Headers.Add("X-Twilio-Signature", CreateValidationHash("http://someothersite" + BaseUrl, _content));

            var response = await _client.SendAsync(request);

            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        private string CreateValidationHash(string url, IDictionary<string, string> parameters)
        {
            var stringBuilder = new StringBuilder(url);
            if (parameters != null)
            {
                var stringList = new List<string>(parameters.Keys);
                stringList.Sort(StringComparer.Ordinal);
                foreach (var key in stringList)
                    stringBuilder.Append(key).Append(parameters[key] ?? "");
            }

            var configurationSection = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json")
                .Build()
                .GetSection("Twilio");

            var hmac = new HMACSHA1(Encoding.UTF8.GetBytes(configurationSection["Token"]));

            return Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(stringBuilder.ToString())));
        }
    }
}