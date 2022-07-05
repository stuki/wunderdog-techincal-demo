using System.Collections.Generic;
using Sula.Core.Models.Entity;
using Sula.Core.Models.Support;

namespace Sula.Core.Models.Response
{
    public class SensorMinMaxDataResponse
    {
        public SensorMinMaxDataResponse(Data data)
        {
            Data = data;
        }

        public Sensor Sensor { get; set; }
        public DataType DataType { get; set; }
        public Data Data { get; set; }
    }

    public class Data {
        public IEnumerable<SensorMinMaxData> Week { get; set; }
        public IEnumerable<SensorMinMaxData> Month { get; set; }
        public IEnumerable<SensorMinMaxData> Year { get; set; }
    }
}