using System.Collections.Generic;
using System.Threading.Tasks;
using Sula.Core.Models;
using Sula.Core.Models.Entity;
using Sula.Core.Models.Request;

namespace Sula.Core.Services.Interfaces
{
    public interface ITextMessageService
    {
        Task SendMessageAsync(Sensor sensor, List<(SensorData data, Limit limit)> brokenLimits);
        Task SendConfirmationMessageAsync(string token, PhoneRequest phoneRequest);
    }
}