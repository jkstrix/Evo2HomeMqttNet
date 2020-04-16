using System;
using Newtonsoft.Json;

namespace Evo2HomeMqttNet.Models.Location
{
    public partial class SetpointCapabilities
    {
        [JsonProperty("maxHeatSetpoint")]
        public long MaxHeatSetpoint { get; set; }

        [JsonProperty("minHeatSetpoint")]
        public long MinHeatSetpoint { get; set; }

        [JsonProperty("valueResolution")]
        public double ValueResolution { get; set; }

        [JsonProperty("canControlHeat")]
        public bool CanControlHeat { get; set; }

        [JsonProperty("canControlCool")]
        public bool CanControlCool { get; set; }

        [JsonProperty("allowedSetpointModes")]
        public SetpointMode[] AllowedSetpointModes { get; set; }

        [JsonProperty("maxDuration")]
        public string MaxDuration { get; set; }

        [JsonProperty("timingResolution")]
        public DateTimeOffset TimingResolution { get; set; }
    }
}
