using Newtonsoft.Json;

namespace Evo2HomeMqttNet.Models.Status
{
    public partial class TemperatureControlSystem
    {
        public TemperatureControlSystem()
        {
        }

        [JsonProperty("systemId")]
        public string SystemId { get; set; }

        [JsonProperty("zones")]
        public Zone[] Zones { get; set; }

        [JsonProperty("activeFaults")]
        public object[] ActiveFaults { get; set; }

        [JsonProperty("systemModeStatus")]
        public SystemModeStatus SystemModeStatus { get; set; }
    }
}