using System;
using System.Collections.Generic;
using Evo2HomeMqttNet.Models;

namespace Evo2HomeMqttNet
{
    public class ConfiguredDevices : CacheObject
    {
        public override TimeSpan Expiry => TimeSpan.MaxValue;

        public List<string> ZoneIds { get; set; }
    }
}