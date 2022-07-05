using System;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sula.AzureFunctions.Functions;
using Sula.Core.Configurations;
using Sula.Core.Extensions;
using Sula.Core.Platform;
using Sula.Core.Services;
using Sula.Core.Services.Interfaces;

[assembly: FunctionsStartup(typeof(Sula.AzureFunctions.Startup))]

namespace Sula.AzureFunctions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("local.settings.json", true, true)
                .AddEnvironmentVariables()
                .Build();

            builder.Services.Configure<TwilioOptions>(options =>
            {
                options.Username = configuration["TwilioAccountSid"];
                options.Password = configuration["TwilioToken"];
                options.PhoneNumber = configuration["TwilioPhoneNumber"];
                options.Domain = configuration["ApiDomain"].TrimUrl();
            });

            builder.Services.Configure<SendGridOptions>(options =>
            {
                options.ApiKey = configuration["SendGridApiKey"];
            });

            builder.Services.AddDbContext<DatabaseContext>(options =>
            {
                options.UseSqlServer(
                    configuration.GetConnectionString("Default"),
                    sqlOptions => { sqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(30), null); }
                );
            });

            builder.Services.AddSingleton<IConfiguration>(services => configuration);
            builder.Services.AddScoped<ITextMessageService, TextMessageService>();
            builder.Services.AddScoped<ISensorProcessingService, SensorProcessingService>();
        }
    }
}