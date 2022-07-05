using System;
using Sula.Core.Models.Support;

namespace Sula.Core.Models.Entity
{
    public class SensorData
    {
        public string DeviceId { get; set; }
        public decimal Value { get; set; }
        public DateTimeOffset Time { get; set; }
        public DataType Type { get; set; }
    }
}