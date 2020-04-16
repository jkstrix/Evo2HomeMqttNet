using Newtonsoft.Json;

namespace Evo2HomeMqttNet.Models.Location
{
    public partial class AllowedSystemMode
    {
        [JsonProperty("systemMode")]
        public string SystemMode { get; set; }

        [JsonProperty("canBePermanent")]
        public bool CanBePermanent { get; set; }

        [JsonProperty("canBeTemporary")]
        public bool CanBeTemporary { get; set; }

        [JsonProperty("maxDuration", NullValueHandling = NullValueHandling.Ignore)]
        public string MaxDuration { get; set; }

        [JsonProperty("timingResolution", NullValueHandling = NullValueHandling.Ignore)]
        public string TimingResolution { get; set; }

        [JsonProperty("timingMode", NullValueHandling = NullValueHandling.Ignore)]
        public string TimingMode { get; set; }
    }
}
