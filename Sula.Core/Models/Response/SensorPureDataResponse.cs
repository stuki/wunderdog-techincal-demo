using System.Collections.Generic;
using System.Linq;
using Sula.Core.Models.Entity;
using Sula.Core.Models.Support;

namespace Sula.Core.Models.Response
{
    public class SensorPureDataResponse
    {
        public SensorPureDataResponse(IEnumerable<SensorData> data)
        {
            Data = data;
            LatestEntry = data.Last();
        }

        public Sensor Sensor { get; set; }
        public DataType DataType { get; set; }
        public IEnumerable<SensorData> Data { get; set; }
        public SensorData LatestEntry { get; set; }
    }
}