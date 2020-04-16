using Newtonsoft.Json;

namespace Evo2HomeMqttNet.Models.Location
{
    public partial class Zone
    {
        [JsonProperty("zoneId")]
        public string ZoneId { get; set; }

        [JsonProperty("modelType")]
        public string ModelType { get; set; }

        [JsonProperty("setpointCapabilities")]
        public SetpointCapabilities SetpointCapabilities { get; set; }

        [JsonProperty("scheduleCapabilities")]
        public ScheduleCapabilities ScheduleCapabilities { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("zoneType")]
        public string ZoneType { get; set; }
    }
}
