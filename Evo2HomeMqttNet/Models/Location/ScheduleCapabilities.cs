using System;
using Newtonsoft.Json;

namespace Evo2HomeMqttNet.Models.Location
{
    public partial class ScheduleCapabilities
    {
        [JsonProperty("maxSwitchpointsPerDay")]
        public long MaxSwitchpointsPerDay { get; set; }

        [JsonProperty("minSwitchpointsPerDay")]
        public long MinSwitchpointsPerDay { get; set; }

        [JsonProperty("timingResolution")]
        public DateTimeOffset TimingResolution { get; set; }

        [JsonProperty("setpointValueResolution")]
        public double SetpointValueResolution { get; set; }
    }
}
