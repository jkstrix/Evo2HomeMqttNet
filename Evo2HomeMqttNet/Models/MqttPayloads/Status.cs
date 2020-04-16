using System;
using Newtonsoft.Json;

namespace Evo2HomeMqttNet.Models.MqttPayloads
{
    public class Status
    {
        [JsonProperty("hold")]
        public string Hold { get; set; }

        [JsonProperty("targetTemp")]
        public string TargetTemp { get; set; }

        [JsonProperty("currentTemp")]
        public string CurrentTemp { get; set; }

        [JsonProperty("action")]
        public string Action { get; set; }

        [JsonProperty("mode")]
        public string Mode { get; set; }

        [JsonProperty("time")]
        public int Time => (int) (DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
    }
}