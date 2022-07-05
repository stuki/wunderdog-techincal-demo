using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Sula.Core.Models.Request;
using Sula.Core.Models.Support;

namespace Sula.Core.Models.Entity
{
    public class Sensor
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public SensorType SensorType { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public List<Limit> Limits { get; set; }
        
        [JsonIgnore]
        public List<SensorData> Data { get; set; }
        
        [JsonIgnore]
        public bool IsActivated { get; set; }

        [JsonIgnore]
        public bool HasEnabledLimits => Limits.Any(limit => limit.IsEnabled);

        public Sensor(string id)
        {
            Id = id;
            SensorType = SensorType.Airwits;
        }

        public void Update(SensorUpdateRequest request)
        {
            Name = request.Name;
        }
    }
}