namespace Sula.Core.Models.Entity
{
    public class Address
    {
        public Address()
        {
        }
        
        public Address( global::Stripe.Address address)
        {
            City = address.City;
            Country = address.Country;
            Line1 = address.Line1;
            Line2 = address.Line2;
            State = address.State;
            PostalCode = address.PostalCode;
        }

        public int Id { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
    }
}