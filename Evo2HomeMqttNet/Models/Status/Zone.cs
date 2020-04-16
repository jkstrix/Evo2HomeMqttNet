using Newtonsoft.Json;

namespace Evo2HomeMqttNet.Models.Status
{
    public partial class Zone
    {
        [JsonProperty("zoneId")]
        public string ZoneId { get; set; }

        [JsonProperty("temperatureStatus")]
        public TemperatureStatus TemperatureStatus { get; set; }

        [JsonProperty("activeFaults")]
        public object[] ActiveFaults { get; set; }

        [JsonProperty("setpointStatus")]
        public SetpointStatus SetpointStatus { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}