using System;
using System.Text.Json.Serialization;
using Sula.Core.Models.Request;
using Sula.Core.Models.Support;

namespace Sula.Core.Models.Entity
{
    public class Limit
    {
        public Limit(SensorLimitAddRequest request)
        {
            DataType = request.DataType;
            Operator = request.Operator;
            Value = request.Value;
            IsEnabled = request.IsEnabled;
        }

        public Limit() {}

        public int Id { get; set; }
        public DataType DataType { get; set; }
        public Operator Operator { get; set; }
        public decimal? Value { get; set; }
        public bool IsEnabled { get; set; }
        public DateTimeOffset? AlertTime { get; set; }

        [JsonIgnore] public bool HasAlertedRecently => AlertTime != null;

        public void UpdateAlertTime()
        {
            AlertTime = DateTimeOffset.Now;
        }

        public void Update(SensorLimitUpdateRequest update)
        {
            DataType = update.DataType;
            Operator = update.Operator;
            Value = update.Value;
            IsEnabled = update.IsEnabled;
            AlertTime = null;
        }
    }
}