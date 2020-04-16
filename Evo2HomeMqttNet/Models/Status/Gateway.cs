using Newtonsoft.Json;

namespace Evo2HomeMqttNet.Models.Status
{
    public partial class Gateway
    {
        [JsonProperty("gatewayId")]
        public string GatewayId { get; set; }

        [JsonProperty("temperatureControlSystems")]
        public TemperatureControlSystem[] TemperatureControlSystems { get; set; }

        [JsonProperty("activeFaults")]
        public object[] ActiveFaults { get; set; }
    }
}