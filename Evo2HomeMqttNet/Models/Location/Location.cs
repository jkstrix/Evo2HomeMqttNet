using System;
using Newtonsoft.Json;

namespace Evo2HomeMqttNet.Models.Location
{
    public class Location : CacheObject
    {
        [JsonProperty("locationInfo")]
        public LocationInfo LocationInfo { get; set; }

        [JsonProperty("gateways")]
        public Gateway[] Gateways { get; set; }

        public override TimeSpan Expiry => TimeSpan.FromDays(1);
    }
}

