using Newtonsoft.Json;

namespace Evo2HomeMqttNet.Models.Location
{
    public partial class LocationInfo
    {
        [JsonProperty("locationId")]
        public string LocationId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("streetAddress")]
        public string StreetAddress { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("postcode")]
        public string Postcode { get; set; }

        [JsonProperty("locationType")]
        public string LocationType { get; set; }

        [JsonProperty("useDaylightSaveSwitching")]
        public bool UseDaylightSaveSwitching { get; set; }

        [JsonProperty("timeZone")]
        public TimeZone TimeZone { get; set; }

        [JsonProperty("locationOwner")]
        public LocationOwner LocationOwner { get; set; }
    }
}
