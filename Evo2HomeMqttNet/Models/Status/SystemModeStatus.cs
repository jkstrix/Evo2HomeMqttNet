using Newtonsoft.Json;

namespace Evo2HomeMqttNet.Models.Status
{
    public partial class SystemModeStatus
    {
        [JsonProperty("mode")]
        public string Mode { get; set; }

        [JsonProperty("isPermanent")]
        public bool IsPermanent { get; set; }
    }
}