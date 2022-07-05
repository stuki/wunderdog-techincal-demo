namespace Sula.Core.Configurations
{
    public class StripeOptions
    {
        public string PublishableKey { get; set; }
        public string SecretKey { get; set; }
        public string WebhookSecret { get; set; }
        public string AirwitsPriceId { get; set; }
        public int AirwitsPrice { get; set; }
        public string BasePriceId { get; set; }
        public double BasePrice { get; set; }
        public string Domain { get; set; }
        public string[] AllowedCountries { get; set; }
    }
}
