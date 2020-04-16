using Newtonsoft.Json;

namespace Evo2HomeMqttNet.Models.Location
{
    public partial class GatewayInfo
    {
        [JsonProperty("gatewayId")]
        public string GatewayId { get; set; }

        [JsonProperty("mac")]
        public string Mac { get; set; }

        [JsonProperty("crc")]
        public string Crc { get; set; }

        [JsonProperty("isWiFi")]
        public bool IsWiFi { get; set; }
    }
}
