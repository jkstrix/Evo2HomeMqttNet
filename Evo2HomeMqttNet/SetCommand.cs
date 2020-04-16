using System;
using Evo2HomeMqttNet.Models.Location;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Evo2HomeMqttNet
{
    public class SetCommand
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public SetpointMode SetPointMode { get; set; }

        public DateTime? TimeUntil { get; set; }

        [JsonConverter(typeof(DoubleJsonConverter))]
        public double HeatSetpointValue { get; set; }
    }
}