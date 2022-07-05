using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sula.Core.Extensions;
using Sula.Core.Models;
using Sula.Core.Models.Entity;
using Sula.Core.Models.Support;
using Sula.Core.Platform;
using Sula.Core.Services.Interfaces;

namespace Sula.Core.Services
{
    public class SensorProcessingService : ISensorProcessingService
    {
        private readonly DatabaseContext _databaseContext;
        private readonly ITextMessageService _textMessageService;

        public SensorProcessingService(DatabaseContext databaseContext, ITextMessageService textMessageService)
        {
            _databaseContext = databaseContext;
            _textMessageService = textMessageService;
        }

        public async Task ProcessSensorDataAsync(HubEvent hubEvent)
        {
            var sensorData = ConverterService.ConvertHubEvent(hubEvent);

            await EvaluateLimitsAsync(hubEvent.DeviceId, sensorData);

            await AddSensorData(hubEvent.DeviceId, sensorData);
        }

        private async Task AddSensorData(string sensorId, IEnumerable<SensorData> sensorData)
        {
            var sensor = await _databaseContext.GetSensor(sensorId) ?? await AddSensor(sensorId);

            sensor.Data ??= new List<SensorData>();

            sensor.Data.AddRange(sensorData);

            await _databaseContext.SaveChangesAsync();
        }

        private async Task<Sensor> AddSensor(string sensorId)
        {
            var sensor = new Sensor(sensorId);
            await _databaseContext.AddAsync(sensor);
            return sensor;
        }


        public async Task EvaluateLimitsAsync(string sensorId, IEnumerable<SensorData> sensorData)
        {
            var sensor = await _databaseContext.GetSensor(sensorId);

            if (sensor == null)
            {
                return;
            }

            if (sensor.HasEnabledLimits)
            {
                var limits = sensor.Limits.Where(limit => limit.IsEnabled && !limit.HasAlertedRecently).ToList();

                var brokenLimits = new List<(SensorData data, Limit limit)>();

                foreach (var data in sensorData)
                {
                    brokenLimits.AddRange(
                        limits
                            .Where(limit => limit.DataType == data.Type && !data.Value.CompareWith(limit))
                            .Select(limit => (data, limit))
                    );
                }

                if (brokenLimits.Count > 0)
                {
                    await _textMessageService.SendMessageAsync(sensor, brokenLimits);
                }
            }
        }
    }
}