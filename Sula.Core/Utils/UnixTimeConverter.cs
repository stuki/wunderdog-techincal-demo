using System;
using Newtonsoft.Json;

namespace Sula.Core.Utils
{
    public class UnixTimeConverter : JsonConverter<DateTimeOffset>
    {
        public override void WriteJson(JsonWriter writer, DateTimeOffset value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToUnixTimeSeconds().ToString());
        }

        public override DateTimeOffset ReadJson(JsonReader reader, Type objectType, DateTimeOffset existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            return DateTimeOffset.FromUnixTimeSeconds(long.Parse((string) reader.Value ?? throw new NullReferenceException()));
        }
    }
}