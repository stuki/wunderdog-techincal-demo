using Newtonsoft.Json;
using Sula.Core.Models.Support;

namespace Sula.Core.Models.Entity
{
    public class Settings
    {
        [JsonIgnore]
        public int Id { get; set; }

        public TemperatureUnit Temperature { get; set; }
        public Language Language { get; set; }
        
        public Settings()
        {
            Temperature = TemperatureUnit.Celsius;
            Language = Language.English;
        }
    }
}