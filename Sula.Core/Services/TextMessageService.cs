using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Sula.Core.Models;
using Sula.Core.Configurations;
using Sula.Core.Models.Entity;
using Sula.Core.Models.Request;
using Sula.Core.Models.Support;
using Sula.Core.Platform;
using Sula.Core.Services.Interfaces;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Sula.Core.Services
{
    public class TextMessageService : ITextMessageService
    {
        private readonly IOptions<TwilioOptions> _options;
        private readonly DatabaseContext _databaseContext;

        public TextMessageService(IOptions<TwilioOptions> options, DatabaseContext databaseContext)
        {
            _options = options;
            _databaseContext = databaseContext;
            TwilioClient.Init(options.Value.Username, options.Value.Password);
        }

        public async Task SendMessageAsync(Sensor sensor, List<(SensorData data, Limit limit)> brokenLimits)
        {
            var user = await _databaseContext.Users.SingleAsync(applicationUser => applicationUser.Id == sensor.UserId);

            var name = sensor.Name ?? sensor.Id;

            var message = await MessageResource.CreateAsync(
                body: $"Your sensor {name} has exceeded the defined threshold. Please check the app for more information",
                from: new PhoneNumber(_options.Value.PhoneNumber),
                to: new PhoneNumber(user.PhoneNumber),
                statusCallback: new Uri(_options.Value.Domain + "/api/v1/webhook/twilio")
            );
            
            var log = new TextMessage(message.Sid, MessageType.Alert);

            await _databaseContext.TextMessages.AddAsync(log);

            brokenLimits.ForEach(tuple => tuple.limit.UpdateAlertTime());

            await _databaseContext.SaveChangesAsync();
        }

        public async Task SendConfirmationMessageAsync(string token, PhoneRequest request)
        {
            var message = await MessageResource.CreateAsync(
                body: "Your Mökki.io confirmation code: " + token,
                from: new PhoneNumber(_options.Value.PhoneNumber),
                to: new PhoneNumber(request.PhoneNumber),
                statusCallback: new Uri(_options.Value.Domain + "/api/v1/webhook/twilio")
            );
            
            var log = new TextMessage(message.Sid, MessageType.Test);

            await _databaseContext.TextMessages.AddAsync(log);

            await _databaseContext.SaveChangesAsync();
        }
    }
}