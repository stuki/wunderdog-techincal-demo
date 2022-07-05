using System.Collections.Generic;

namespace Sula.Core.Configurations
{
    public class SendGridOptions
    {
        public string ApiKey { get; set; }
        public string FromEmail { get; set; }
        public string FromName { get; set; }
        public Dictionary<string, string> Templates { get; set; }
    }
}