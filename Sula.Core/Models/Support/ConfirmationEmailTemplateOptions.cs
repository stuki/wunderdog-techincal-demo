using Newtonsoft.Json;

namespace Sula.Core.Models.Support
{
    public class ConfirmationEmailTemplateOptions
    {
        [JsonProperty("confirmation_url")]
        public string ConfirmationUrl { get; set; }
    }
}