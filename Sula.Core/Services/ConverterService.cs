using System.Globalization;
using Sula.Core.Models;
using Sula.Core.Models.Entity;
using Sula.Core.Models.Support;

namespace Sula.Core.Services
{
    public static class ConverterService
    {
        public static SensorData[] ConvertHubEvent(HubEvent hubEvent)
        {
            return new[]
            {
                new SensorData
                {
                    DeviceId = hubEvent.DeviceId,
                    Time = hubEvent.Time,
                    Type = DataType.Temperature,
                    Value = int.Parse(hubEvent.Data.Substring(2, 4), NumberStyles.HexNumber) / (decimal) 10 - 40
                },
                new SensorData
                {
                    DeviceId = hubEvent.DeviceId,
                    Time = hubEvent.Time,
                    Type = DataType.Humidity,
                    Value = int.Parse(hubEvent.Data.Substring(6, 2), NumberStyles.HexNumber)
                }
            };
        }
    }
}