using Newtonsoft.Json;

namespace Evo2HomeMqttNet.Models.Location
{
    public partial class Gateway
    {
        [JsonProperty("gatewayInfo")]
        public GatewayInfo GatewayInfo { get; set; }

        [JsonProperty("temperatureControlSystems")]
        public TemperatureControlSystem[] TemperatureControlSystems { get; set; }
    }
}


