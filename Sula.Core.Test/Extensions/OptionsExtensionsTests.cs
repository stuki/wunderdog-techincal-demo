using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Sula.Core.Configurations;
using Sula.Core.Extensions;
using Xunit;

namespace Sula.Core.Test.Extensions
{
    public class OptionsExtensionsTests
    {
        [Fact]
        public void ConfigureCustomOptions_Should_Populate_TwilioOptions_Correctly()
        {
            var options = GetOptionsProvider<IOptions<TwilioOptions>>();
            
            Assert.NotNull(options.Value.Domain);
            Assert.NotNull(options.Value.Username);
            Assert.NotNull(options.Value.Password);
            Assert.NotNull(options.Value.PhoneNumber);
        }
        
        [Fact]
        public void ConfigureCustomOptions_Should_Populate_SendgridOptions_Correctly()
        {
            var options = GetOptionsProvider<IOptions<SendGridOptions>>();
            
            Assert.NotEmpty(options.Value.Templates);
            Assert.NotNull(options.Value.Templates["Registration"]);
            Assert.NotNull(options.Value.Templates["Reset"]);
            Assert.NotNull(options.Value.Templates["Welcome"]);
            Assert.NotNull(options.Value.ApiKey);
            Assert.NotNull(options.Value.FromEmail);
            Assert.NotNull(options.Value.FromName);
        }
        
        [Fact]
        public void ConfigureCustomOptions_Should_Populate_StripeOptions_Correctly()
        {
            var options = GetOptionsProvider<IOptions<StripeOptions>>();
            
            Assert.NotNull(options.Value.Domain);
            Assert.NotEmpty(options.Value.AllowedCountries);
            Assert.NotNull(options.Value.AirwitsPriceId);
            Assert.NotNull(options.Value.BasePriceId);
            Assert.True(options.Value.AirwitsPrice != 0);
            Assert.True(options.Value.BasePrice != 0);
            Assert.NotNull(options.Value.PublishableKey);
            Assert.NotNull(options.Value.SecretKey);
            Assert.NotNull(options.Value.WebhookSecret);
        }
        
        [Fact]
        public void ConfigureCustomOptions_Should_Populate_TwilioOptions_Correctly_WithEnvironmentVariables()
        {
            var options = GetOptionsProviderWithEnvironmentVariables<IOptions<TwilioOptions>>();
            
            Assert.NotNull(options.Value.Domain);
            Assert.NotNull(options.Value.Username);
            Assert.NotNull(options.Value.Password);
            Assert.NotNull(options.Value.PhoneNumber);
        }
        
        [Fact]
        public void ConfigureCustomOptions_Should_Populate_SendgridOptions_Correctly_WithEnvironmentVariables()
        {
            var options = GetOptionsProviderWithEnvironmentVariables<IOptions<SendGridOptions>>();
            
            Assert.NotEmpty(options.Value.Templates);
            Assert.NotNull(options.Value.Templates["Registration"]);
            Assert.NotNull(options.Value.Templates["Reset"]);
            Assert.NotNull(options.Value.Templates["Welcome"]);
            Assert.NotNull(options.Value.ApiKey);
            Assert.NotNull(options.Value.FromEmail);
            Assert.NotNull(options.Value.FromName);
        }
        
        [Fact]
        public void ConfigureCustomOptions_Should_Populate_StripeOptions_Correctly_WithEnvironmentVariables()
        {
            var options = GetOptionsProviderWithEnvironmentVariables<IOptions<StripeOptions>>();
            
            Assert.NotNull(options.Value.Domain);
            Assert.NotEmpty(options.Value.AllowedCountries);
            Assert.NotNull(options.Value.AirwitsPriceId);
            Assert.NotNull(options.Value.BasePriceId);
            Assert.True(options.Value.AirwitsPrice != 0);
            Assert.True(options.Value.BasePrice != 0);
            Assert.NotNull(options.Value.PublishableKey);
            Assert.NotNull(options.Value.SecretKey);
            Assert.NotNull(options.Value.WebhookSecret);
        }

        private static T GetOptionsProvider<T>()
        {
            var serviceCollection = new ServiceCollection();
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory() + "/../../../../Sula.Api/")
                .AddJsonFile("appsettings.Development.json")
                .Build();
            
            serviceCollection.ConfigureCustomOptions(configuration);

            var provider = serviceCollection.BuildServiceProvider();

            return (T) provider.GetService(typeof(T));
        }
        private static T GetOptionsProviderWithEnvironmentVariables<T>()
        {
            Environment.SetEnvironmentVariable("AllowedCountries:0", "FI");
            Environment.SetEnvironmentVariable("AllowedCountries:1", "SE");
            Environment.SetEnvironmentVariable("Domain", "http:/localhost");
            Environment.SetEnvironmentVariable("SendGrid:ApiKey", "12341234");
            Environment.SetEnvironmentVariable("SendGrid:FromEmail", "valid.user@example.com");
            Environment.SetEnvironmentVariable("SendGrid:FromName", "Mokki");
            Environment.SetEnvironmentVariable("SendGrid:Templates:Registration", "12341234");
            Environment.SetEnvironmentVariable("SendGrid:Templates:Reset", "12341234");
            Environment.SetEnvironmentVariable("SendGrid:Templates:Welcome", "12341234");
            Environment.SetEnvironmentVariable("Stripe:AirwitsPrice", "1");
            Environment.SetEnvironmentVariable("Stripe:AirwitsPriceId", "12341234");
            Environment.SetEnvironmentVariable("Stripe:BasePrice", "1");
            Environment.SetEnvironmentVariable("Stripe:BasePriceId", "12341234");
            Environment.SetEnvironmentVariable("Stripe:PublishableKey", "1241234");
            Environment.SetEnvironmentVariable("Stripe:SecretKey", "12341234");
            Environment.SetEnvironmentVariable("Stripe:WebhookSecret", "12341234");
            Environment.SetEnvironmentVariable("Twilio:AccountSid", "12341234");
            Environment.SetEnvironmentVariable("Twilio:PhoneNumber", "+424242424242");
            Environment.SetEnvironmentVariable("Twilio:Token", "12341234");
            
            var serviceCollection = new ServiceCollection();
            var configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();
            
            serviceCollection.ConfigureCustomOptions(configuration);

            var provider = serviceCollection.BuildServiceProvider();

            return (T) provider.GetService(typeof(T));
        }
    }
}