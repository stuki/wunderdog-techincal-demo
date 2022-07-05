namespace Sula.Core.Models.Stripe
{
    public class ErrorMessage
    {
        public string Message { get; set; }
    }

    public class ErrorResponse
    {
        public ErrorMessage Error { get; set; }
    }
}