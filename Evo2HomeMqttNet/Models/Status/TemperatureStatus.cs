using Newtonsoft.Json;

namespace Evo2HomeMqttNet.Models.Status
{
    public partial class TemperatureStatus
    {
        [JsonProperty("temperature")]
        public double Temperature { get; set; }

        [JsonProperty("isAvailable")]
        public bool IsAvailable { get; set; }
    }
}