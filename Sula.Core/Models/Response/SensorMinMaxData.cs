using System;
using Sula.Core.Models.Support;

namespace Sula.Core.Models.Response
{
    public class SensorMinMaxData
    {
        public string DeviceId { get; set; }
        public DateTimeOffset Time { get; set; }
        public DataType Type { get; set; }
        public decimal Min { get; set; }
        public decimal Max { get; set; }
        public decimal Mean { get; set; }

    }
}