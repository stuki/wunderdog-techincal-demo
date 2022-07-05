using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Stripe;
using Sula.Core.Models.Request;

namespace Sula.Core.Models.Entity
{
    public sealed class ApplicationUser : IdentityUser
    {
        public List<Sensor> Sensors { get; set; }
        public Settings Settings { get; set; }
        public string StripeCustomerId { get; set; }
        public string StripeSubscriptionId { get; set; }
        public Address Address { get; set; }

        public ApplicationUser()
        {
        }

        public ApplicationUser(Customer customer, global::Stripe.Address address, string subscriptionId)
        {
            Address = new Address(address);
            Email = customer.Email;
            UserName = customer.Email;
            PhoneNumber = customer.Phone;
            StripeCustomerId = customer.Id;
            StripeSubscriptionId = subscriptionId;
            Settings = new Settings();
        }

        public void Update(ApplicationUser updatedUser)
        {
            Settings = updatedUser.Settings;
        }
    }
}