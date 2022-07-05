using System;
using Newtonsoft.Json;
using Sula.Core.Utils;

namespace Sula.Core.Models.Support
{
    public class HubEvent
    {
        public string DeviceId { get; set; }
        public string Data { get; set; }
        [JsonConverter(typeof(UnixTimeConverter))]
        public DateTimeOffset Time { get; set; }
        public int SequenceNumber { get; set; }
    }
}