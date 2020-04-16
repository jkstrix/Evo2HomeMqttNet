using Newtonsoft.Json;

namespace Evo2HomeMqttNet.Models.MqttPayloads
{
    public class Device
    {
        [JsonProperty("identifiers")]
        public string[] Identifiers { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("sw_version")]
        public string SwVersion { get; set; }

        [JsonProperty("model")]
        public string Model { get; set; }

        [JsonProperty("manufacturer")]
        public string Manufacturer { get; set; }
    }
}