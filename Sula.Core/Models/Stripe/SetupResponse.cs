namespace Sula.Core.Models.Stripe
{
    public class SetupResponse
    {
        public string PublishableKey { get; set; }
        public string AirwitsPriceId { get; set; }
        public int AirwitsPrice { get; set; }
        public double BasePrice { get; set; }
    }
}
