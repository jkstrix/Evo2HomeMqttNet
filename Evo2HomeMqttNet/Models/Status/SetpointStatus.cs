using Evo2HomeMqttNet.Models.Location;
using Newtonsoft.Json;

namespace Evo2HomeMqttNet.Models.Status
{
    public partial class SetpointStatus
    {
        [JsonProperty("targetHeatTemperature")]
        public double TargetHeatTemperature { get; set; }

        [JsonProperty("setpointMode")]
        public SetpointMode SetpointMode { get; set; }
    }
}