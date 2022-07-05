using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Sula.Core.Exceptions;
using Sula.Core.Extensions;
using Sula.Core.Models;
using Sula.Core.Models.Support;
using Sula.Core.Services;
using Sula.Core.Services.Interfaces;
using Newtonsoft.Json;
using IoTHubTrigger = Microsoft.Azure.WebJobs.EventHubTriggerAttribute;

namespace Sula.AzureFunctions.Functions
{
    public class DataProcessingFunction
    {
        private readonly ISensorProcessingService _sensorProcessingService;

        public DataProcessingFunction(ISensorProcessingService sensorProcessingService)
        {
            _sensorProcessingService = sensorProcessingService;
        }

        [FunctionName("ProcessAndStoreData")]
        public async Task Run(
            [IoTHubTrigger("messages/events", Connection = "HubEndPoint")]
            EventData[] messages, ILogger log)
        {
            var guid = Guid.NewGuid().ToString();
            log.LogInformation($"Processing batch {guid} of size {messages.Length}");
            
            foreach (var message in messages)
            {
                var dataString = Encoding.UTF8.GetString(message.Body.Array ?? throw new ArgumentNullException());

                try
                {
                    var hubEvent = JsonConvert.DeserializeObject<HubEvent>(dataString);

                    if (hubEvent.IsEmpty())
                    {
                        throw new DeserializeException();
                    }
                    
                    await _sensorProcessingService.ProcessSensorDataAsync(hubEvent);
                }
                catch (Exception exception)
                {
                    log.LogError(exception, "An error occured during processing event data: {DataString}", dataString);
                    throw;
                }

                log.LogInformation($"Batch {guid} processed");
            }
        }
        
    }
}