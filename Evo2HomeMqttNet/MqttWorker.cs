using System;
using System.Text;
using System.Threading.Tasks;
using Evo2HomeMqttNet.Models;
using Evo2HomeMqttNet.Models.MqttPayloads;
using Evo2HomeMqttNet.Models.Status;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client.Options;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Protocol;
using Newtonsoft.Json;

namespace Evo2HomeMqttNet
{
    public class MqttWorker
    {
        private readonly EvoHomeSettings _settings;
        private readonly ILogger<MqttWorker> _logger;
        private readonly IManagedMqttClient _mqttClient;
        private readonly ManagedMqttClientOptions _managedOptions;

        public MqttWorker(EvoHomeSettings evoHomeSettings, ILogger<MqttWorker> logger)
        {
            _settings = evoHomeSettings;
            _logger = logger;

            if (evoHomeSettings.DisableMqtt)
            {
                _logger.LogInformation("Mqqt Disabled");
                return;
            }

            var options = new MqttClientOptionsBuilder()
                .WithClientId(evoHomeSettings.MqttClientName)
                .WithCredentials(evoHomeSettings.MqttUser, evoHomeSettings.MqttPassword)
                .WithTcpServer(evoHomeSettings.MqttConnection, evoHomeSettings.MqttPort)
                .Build();

            _managedOptions = new ManagedMqttClientOptionsBuilder()
                .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
                .WithClientOptions(options)
                .Build();

            var factory = new MqttFactory();
            _mqttClient = factory.CreateManagedMqttClient();
        }

        public Func<string, string, Task> OnReceived { get; set; }

        public async Task ConnectAsync()
        {
            try
            {
                _mqttClient.UseConnectedHandler(e => { _logger.LogInformation("Connected to Mqtt Broker"); });
                _mqttClient.UseDisconnectedHandler(e =>
                {
                    _logger.LogWarning("Disconnected from Mqtt Broker", e);
                });

                _mqttClient.UseApplicationMessageReceivedHandler(async e =>
                {
                    try
                    {
                        var topic = e.ApplicationMessage.Topic;
                        if (!string.IsNullOrWhiteSpace(topic))
                        {
                            var payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                            _logger.LogInformation($"Topic: {topic}. Message Received: {payload}");
                            await OnReceived(topic, payload);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message, ex);
                    }
                });
                await _mqttClient.StartAsync(_managedOptions);
                
            }
            catch (Exception e)
            {
                _logger.LogError("Failed to start Mqtt Client", e);
                throw;
            }
        }

        public async Task Publish(string topic, string payload)
        {
            _logger.LogInformation($"Publishing Topic: {topic} Payload: {payload}");
            await _mqttClient.PublishAsync(new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(payload)
                .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.ExactlyOnce)
                .WithRetainFlag()
                .Build());
        }

        public async Task SubscribeToTopicAsync(string topic)
        {
            
            _logger.LogDebug($"Subscribing to topic {topic}");
            await _mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic(topic)
                .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce).Build());
        }

        public async Task PublishDiscoveryConfig(string zoneName, string zoneId, string manufacturer, string model,
            string version)
        {
            var topic = $"{_settings.MqqtDiscoveryPrefix}/climate/{zoneId}/config";
            var disco = new DiscoveryObject(zoneName, zoneId, _settings.MqttPrefix, manufacturer, model, version);

            await Publish(topic, JsonConvert.SerializeObject(disco));
        }
    }
}
