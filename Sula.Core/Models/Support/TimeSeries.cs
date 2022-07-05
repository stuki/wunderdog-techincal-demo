using System;
using Sula.Core.Models.Entity;

namespace Sula.Core.Models.Support
{
    public class TimeSeries
    {
        public TimeSeries() {}
        
        public TimeSeries(SensorData sensorData)
        {
            Value = sensorData.Value;
            Time = sensorData.Time;
        }

        public decimal Value { get; set; }
        public DateTimeOffset Time { get; set; }
    }
}