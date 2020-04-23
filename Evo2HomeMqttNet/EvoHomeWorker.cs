using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Evo2HomeMqttNet.Models;
using Evo2HomeMqttNet.Models.Location;
using Evo2HomeMqttNet.Models.MqttPayloads;
using Evo2HomeMqttNet.Models.Status;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestEase;
using Zone = Evo2HomeMqttNet.Models.Status.Zone;

namespace Evo2HomeMqttNet
{
    public class EvoHomeWorker : IDisposable
    {
        private readonly EvoHomeSettings _evoHomeSettings;
        private readonly ILogger<EvoHomeWorker> _logger;
        private readonly TokenManager _manager;
        private readonly MqttWorker _mqttWorker;
        private readonly Store _store;
        private Location _location;
        private LocationStatus _status;
        private readonly ConfiguredDevices _configuredDevices;

        public object Lockable = new object();

        public EvoHomeWorker(TokenManager manager, EvoHomeSettings evoHomeSettings, Store store,
            ILogger<EvoHomeWorker> logger, MqttWorker mqttWorker, IHttpClientFactory clientFactory)
        {
            _manager = manager;
            _evoHomeSettings = evoHomeSettings;
            _store = store;
            _logger = logger;
            _mqttWorker = mqttWorker;
            Client = RestClient.For<IEvoClient>(clientFactory.CreateClient(Constants.EvoClientName));

            _configuredDevices =
                _store.Get<ConfiguredDevices>() ?? new ConfiguredDevices() {ZoneIds = new List<string>()};
        }

        public IEvoClient Client { get; }

        public Timer Timer { get; set; }

        public async Task Start()
        {
            if (!_evoHomeSettings.DisableMqtt)
            {
                await _mqttWorker.ConnectAsync();
                _mqttWorker.OnReceived = PayloadReceivedAsync;
            }

            try
            {
                var token = await _manager.GetToken(_evoHomeSettings.EvoUsername, _evoHomeSettings.EvoPassword);
                Client.AuthHeader = $"{token.TokenType} {token.AccessToken}";
            }
            catch (Exception e)
            {
                _logger.LogError("Cannot get authentication Token", e);
                throw;
            }
            
            _logger.LogInformation("Getting User Account Information");
            var userAccount = await GetStoreValueOrResult(async () => await Client.GetUserAccountAsync());

            _logger.LogInformation($"Location Information For {_evoHomeSettings.LocationName}");
            _location = await GetStoreValueOrResult(async () =>
                (await Client.GetLocationAsync(userAccount.UserId, true)).Single(x =>
                    x.LocationInfo.Name.Equals(_evoHomeSettings.LocationName,
                        StringComparison.InvariantCultureIgnoreCase)));

            await GetLocationStatusAsync();

            if (!_evoHomeSettings.DisableMqtt)
                foreach (var zone in _status.Gateways.SelectMany(x => x.TemperatureControlSystems)
                    .SelectMany(x => x.Zones))
                {
                    await _mqttWorker.SubscribeToTopicAsync(
                        $"{_evoHomeSettings.MqttPrefix}/climate/{zone.ZoneId}/set/+");

                    if (_evoHomeSettings.MqttDiscovery &&
                        string.IsNullOrWhiteSpace(_evoHomeSettings.MqttDiscoveryPrefix) == false)
                    {
                        if (!_configuredDevices.ZoneIds.Contains(zone.ZoneId))
                        {    
                            var zoneInfo = _location.Gateways.SelectMany(x => x.TemperatureControlSystems)
                                .SelectMany(x => x.Zones).Single(x => x.ZoneId == zone.ZoneId);

                            await _mqttWorker.PublishDiscoveryConfig(zone.Name, zone.ZoneId, Constants.Manufacturer,
                                zoneInfo.ModelType, Constants.Version);
                            _configuredDevices.ZoneIds.Add(zone.ZoneId);
                            _store.AddOrUpdate(_configuredDevices);
                        }
                    }
                }

            Timer = new Timer(
                GetLocationStatusOnTimer,
                null,
                _evoHomeSettings.PollRate,
                _evoHomeSettings.PollRate);
        }

        public async Task PayloadReceivedAsync(string topic, string payload)
        {
            if (await TrySendCommandAsync(topic, payload)) await GetLocationStatusAsync();
        }

        private async Task<bool> TrySendCommandAsync(string topic, string payload)
        {
            try
            {
                var topicSplit = topic.Split("/");
                var zoneId = topicSplit[2];
                var command = topicSplit[4];

                var currentStatus = _status.Gateways.SelectMany(x => x.TemperatureControlSystems)
                    .SelectMany(x => x.Zones)
                    .Select(x => new
                    {
                        x.Name, x.ZoneId, x.SetpointStatus.TargetHeatTemperature, x.SetpointStatus.SetpointMode,
                        x.TemperatureStatus.Temperature
                    }).Single(x => x.ZoneId == zoneId);

                var token = await _manager.GetToken(_evoHomeSettings.EvoUsername, _evoHomeSettings.EvoPassword);
                Client.AuthHeader = $"{token.TokenType} {token.AccessToken}";

                SetCommand setCommand;

                switch (command.ToLower())
                {
                    case "holdcmd":
                        setCommand = GetHoldCommand(currentStatus.TargetHeatTemperature, payload);
                        break;
                    case "targettempcmd":
                        setCommand = GetTemperatureCommand(currentStatus.SetpointMode, payload);
                        break;
                    default:
                        return false;
                }

                _logger.LogInformation($"Setting Zone State for Zone {currentStatus.Name}");
                await Client.HeatSeatPointAsync(zoneId, setCommand);
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError("Failed to send Set Heat Point Command", e);
                return false;
            }
        }

        private SetCommand GetTemperatureCommand(SetpointMode setpointMode, string payload)
        {
            if (setpointMode == SetpointMode.FollowSchedule || setpointMode == SetpointMode.PermanentOverride)
                return new SetCommand
                {
                    SetPointMode = SetpointMode.PermanentOverride,
                    HeatSetpointValue = double.Parse(payload)
                };

            return new SetCommand
            {
                SetPointMode = SetpointMode.TemporaryOverride, HeatSetpointValue = double.Parse(payload),
                TimeUntil = DateTime.Now.AddHours(2).ToUniversalTime()
            };
        }

        private SetCommand GetHoldCommand(double temperature, string payload)
        {
            if (payload.Equals("off", StringComparison.InvariantCultureIgnoreCase))
                return new SetCommand {SetPointMode = SetpointMode.FollowSchedule, HeatSetpointValue = 0};

            if (payload.Equals("Permanent", StringComparison.InvariantCultureIgnoreCase))
                return new SetCommand {SetPointMode = SetpointMode.PermanentOverride, HeatSetpointValue = temperature};

            return new SetCommand
            {
                SetPointMode = SetpointMode.TemporaryOverride, HeatSetpointValue = temperature,
                TimeUntil = DateTime.Now.AddHours(2).ToUniversalTime()
            };
        }

        private void GetLocationStatusOnTimer(object state)
        {
            lock (Lockable)
            {
                _logger.LogInformation("Polling for status");
                GetLocationStatusAsync().GetAwaiter().GetResult();
            }
        }

        private async Task GetLocationStatusAsync()
        {
            if (_evoHomeSettings.DisableMqtt)
            {
                return;
            }

            try
            {
                var token = await _manager.GetToken(_evoHomeSettings.EvoUsername, _evoHomeSettings.EvoPassword);
                Client.AuthHeader = $"{token.TokenType} {token.AccessToken}";
                _logger.LogInformation($"Retrieving Location Status For {_location.LocationInfo.Name}");
                _status = await Client.GetLocationStatusAsync(_location.LocationInfo.LocationId, true);
                _logger.LogInformation($"Retrieved Location Status For {_location.LocationInfo.Name}");

                foreach (var zone in _status.Gateways.SelectMany(x => x.TemperatureControlSystems)
                    .SelectMany(x => x.Zones))
                {
                    _logger.LogInformation($"Publishing updated status for zone {zone.Name}");
                    await Publish(zone.ZoneId, ZoneToStatus(zone));
                }
            }
            catch (Exception e)
            {
               _logger.LogError("Failed to retrieve status updates", e);
            }
        }

        private Status ZoneToStatus(Zone zone)
        {
            return new Status
            {
                Action = MapAction(zone.SetpointStatus.TargetHeatTemperature, zone.TemperatureStatus.Temperature),
                CurrentTemp = zone.TemperatureStatus.Temperature.ToString(Constants.NumberFormat),
                Hold = MapHoldState(zone.SetpointStatus.SetpointMode),
                Mode = Constants.ModeHeat,
                TargetTemp = zone.SetpointStatus.TargetHeatTemperature.ToString(Constants.NumberFormat)
            };
        }

        private string MapAction(in double setpointStatusTargetHeatTemperature, in double temperatureStatusTemperature)
        {
            if (setpointStatusTargetHeatTemperature > temperatureStatusTemperature) return Constants.ActionHeating;

            return Constants.ActionIdle;
        }

        private string MapHoldState(SetpointMode setpointStatusSetpointMode)
        {
            switch (setpointStatusSetpointMode)
            {
                case SetpointMode.FollowSchedule:
                    return Constants.HoldModeOff;
                case SetpointMode.PermanentOverride:
                    return Constants.HoldModePermanent;
                default:
                    return Constants.HoldModeTemporary;
            }
        }


        private async Task Publish(string zoneId, Status thermostatStatus)
        {
            var payload = JsonConvert.SerializeObject(thermostatStatus);
            await _mqttWorker.Publish($"{_evoHomeSettings.MqttPrefix}/climate/{zoneId}/state", payload);
        }

        private async Task<T> GetStoreValueOrResult<T>(Func<Task<T>> func) where T : CacheObject, new()
        {
            try
            {
                var value = _store.Get<T>();
                if (value != null) return value;

                value = await func.Invoke();
                _store.AddOrUpdate(value);
                return value;
            }
            catch (StoreException exception)
            {
                _logger.LogError($"Failed to retrieve value from store {typeof(T).FullName}", exception);
                throw;
            }
            catch (StoreSaveException exception)
            {
                _logger.LogError($"Failed to save value to store {typeof(T).FullName}", exception);
                throw;
            }
            catch (Exception exception)
            {
                _logger.LogError($"Failed to get value from TTC {typeof(T).FullName}", exception);
                throw;
            }
        }

        public void Dispose()
        {
            Timer?.Dispose();
        }
    }
}