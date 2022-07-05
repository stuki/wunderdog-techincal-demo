using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sula.Core.Configurations;
using Newtonsoft.Json;
using SendGrid;

namespace Sula.AzureFunctions.Functions
{
    public class AddCustomerToSendgridMarketingFunction
    {
        private readonly IOptions<SendGridOptions> _options;
        public AddCustomerToSendgridMarketingFunction(IOptions<SendGridOptions> options)
        {
            _options = options;
        }
        
        [FunctionName("AddCustomerToSendgridMarketing")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]
            HttpRequest req,
            ILogger log)
        {
            string email = req.Query["email"];

            if (email == null)
            {
                log.LogError("Query did not contain email");
                return new BadRequestObjectResult("Query did not contain email");
            }
            
            log.LogInformation("Triggered with query {Query}", email);

            var client = new SendGridClient(_options.Value.ApiKey);

            // c4f046a5-bc68-4f35-9047-6c1b9135004a => PreLaunch list at SendGrid
            var request = new SendgridContactsRequest(new List<string> { "c4f046a5-bc68-4f35-9047-6c1b9135004a" }, new Contact(email.ToLower()));

            var response = await client.RequestAsync(SendGridClient.Method.PUT, JsonConvert.SerializeObject(request), urlPath: "marketing/contacts");
            
            if (response.StatusCode == HttpStatusCode.Accepted || response.StatusCode == HttpStatusCode.OK)
            {
                log.LogInformation("Successfully sent email {Email} to SendGrid", email);
                return new OkResult();
            }

            log.LogError("Failed to update customer email to SendGrid with status code {StatusCode}", response.StatusCode);
            return new BadRequestObjectResult("Failed to update customer email to SendGrid");
        }

        public class SendgridContactsRequest
        {
            [JsonProperty("list_ids")]
            public List<string> Ids { get; set; }
            
            [JsonProperty("contacts")]
            public List<Contact> Contacts { get; set; }

            public SendgridContactsRequest(List<string> ids, Contact contact)
            {
                Ids = ids;
                Contacts = new List<Contact> { contact };
            }
        }

        public class Contact
        {
            public string Email { get; set; }

            public Contact(string email)
            {
                Email = email;
            }
        }
    }
}
