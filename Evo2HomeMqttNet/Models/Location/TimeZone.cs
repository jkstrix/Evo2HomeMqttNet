using Newtonsoft.Json;

namespace Evo2HomeMqttNet.Models.Location
{
    public partial class TimeZone
    {
        [JsonProperty("timeZoneId")]
        public string TimeZoneId { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("offsetMinutes")]
        public long OffsetMinutes { get; set; }

        [JsonProperty("currentOffsetMinutes")]
        public long CurrentOffsetMinutes { get; set; }

        [JsonProperty("supportsDaylightSaving")]
        public bool SupportsDaylightSaving { get; set; }
    }
}
