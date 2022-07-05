using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sula.Core.Configurations;

namespace Sula.Core.Extensions
{
    public static class OptionsExtension
    {
        public static void ConfigureCustomOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<TwilioOptions>(options =>
            {
                var section = configuration.GetSection("Twilio");

                options.Username = section["AccountSid"];
                options.Password = section["Token"];
                options.PhoneNumber = section["PhoneNumber"];
                options.Domain = configuration.GetValue<string>("Domain").TrimUrl();
            });

            services.Configure<StripeOptions>(options =>
            {
                var section = configuration.GetSection("Stripe");

                options.PublishableKey = section["PublishableKey"];
                options.SecretKey = section["SecretKey"];
                options.WebhookSecret = section["WebhookSecret"];
                options.AirwitsPriceId = section["AirwitsPriceId"];
                options.AirwitsPrice = int.Parse(section["AirwitsPrice"]);
                options.BasePriceId = section["BasePriceId"];
                options.BasePrice = double.Parse(section["BasePrice"]);
                options.Domain = configuration.GetValue<string>("Domain").TrimUrl();
                options.AllowedCountries = configuration.GetSection("AllowedCountries").Get<string[]>();
            });

            services.Configure<SendGridOptions>(options =>
            {
                var section = configuration.GetSection("SendGrid");

                options.ApiKey = section["ApiKey"];
                options.FromEmail = section["FromEmail"];
                options.FromName = section["FromName"];
                options.Templates = section.GetSection("Templates").GetChildren()
                    .ToDictionary(template => template.Key, template => template.Value);
            });
        }
    }
}