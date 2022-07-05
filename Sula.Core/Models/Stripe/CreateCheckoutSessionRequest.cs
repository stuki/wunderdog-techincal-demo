namespace Sula.Core.Models.Stripe
{
    public class CreateCheckoutSessionRequest
    {
        public string PriceId { get; set; }
        public int Quantity { get; set; }
    }
}