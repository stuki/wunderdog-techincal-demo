using System;
using Sula.Core.Models.Entity;
using Sula.Core.Models.Support;

namespace Sula.Core.Models
{
    public class Alert
    {
        public string Name { get; set; }
        public DateTimeOffset? AlertTime { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public DataType DataType { get; set; }
        public Operator Operator { get; set; }
        public decimal? Value { get; set; }

        public Alert(Limit limit, Sensor sensor)
        {
            Name = sensor.Name ?? sensor.Id;
            AlertTime = limit.AlertTime;
            Latitude = sensor.Latitude;
            Longitude = sensor.Longitude;
            Operator = limit.Operator;
            DataType = limit.DataType;
            Value = limit.Value;
        }
    }
}   