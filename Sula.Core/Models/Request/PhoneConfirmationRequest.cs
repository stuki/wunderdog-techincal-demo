namespace Sula.Core.Models.Request
{
    public class PhoneConfirmationRequest
    {
        public string ConfirmationToken { get; set; }
        public string PhoneNumber { get; set; }
    }
}