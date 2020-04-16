using Newtonsoft.Json;

namespace Evo2HomeMqttNet.Models.Status
{
    public partial class LocationStatus
    {
        [JsonProperty("locationId")]
        public string LocationId { get; set; }

        [JsonProperty("gateways")]
        public Gateway[] Gateways { get; set; }
       
    }
}