using Newtonsoft.Json;

namespace Evo2HomeMqttNet.Models.Location
{
    public partial class TemperatureControlSystem
    {
        [JsonProperty("systemId")]
        public string SystemId { get; set; }

        [JsonProperty("modelType")]
        public string ModelType { get; set; }

        [JsonProperty("zones")]
        public Zone[] Zones { get; set; }

        [JsonProperty("allowedSystemModes")]
        public AllowedSystemMode[] AllowedSystemModes { get; set; }
    }
}
