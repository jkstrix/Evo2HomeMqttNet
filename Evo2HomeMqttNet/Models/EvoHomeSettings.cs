using System;

namespace Evo2HomeMqttNet.Models
{
    public class EvoHomeSettings
    {
        public string EvoUsername { get; set; }

        public string EvoPassword { get; set; }
        public string LocationName { get; set; }
        public TimeSpan PollRate { get; set; }

        public string MqttUser { get; set; }

        public string MqttPassword { get; set; }

        public string MqttPrefix { get; set; }

        public string MqttConnection { get; set; }
        public string MqttClientName { get; set; }

        public int MqttPort { get; set; }
        public bool DisableMqtt { get; set; }
        public bool MqqtDiscovery { get; set; }
        public string MqqtDiscoveryPrefix { get; set; }
        public string FileLocation { get; set; }
    }
}