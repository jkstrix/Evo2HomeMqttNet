using System.Reflection.Metadata;
using Newtonsoft.Json;

namespace Evo2HomeMqttNet.Models.MqttPayloads
{
    public class DiscoveryObject
    {
        private readonly string _zoneName;
        private readonly string _zoneId;
        private readonly string _mqttPrefix;

        [JsonIgnore]
        public string StateLocation => $"{_mqttPrefix}/climate/{_zoneId}/state";

        public DiscoveryObject(string zoneName, string zoneId, string mqttPrefix, string manufacturer, string model,
            string version)
        {
            _zoneName = zoneName;
            _zoneId = zoneId;
            _mqttPrefix = mqttPrefix;
            Device = new Device()
            {
                Name = _zoneName,
                Identifiers = new[] {$"{_mqttPrefix}_{_zoneId}"},
                Manufacturer = manufacturer,
                Model = model,
                SwVersion = version
            };
        }

        [JsonProperty("uniq_id")] public string UniqueId => $"Evo_{_mqttPrefix}_{_zoneId}";

        [JsonProperty("name")] public string Name => $"{_zoneName} {Constants.Thermostat}";

        [JsonProperty("hold_cmd_t")] public string HoldCmdT => $"{_mqttPrefix}/climate/{_zoneId}/set/holdCmd";

        [JsonProperty("hold_stat_t")] public string HoldStatT => StateLocation;

        [JsonProperty("hold_stat_tpl")] public string HoldStatTpl => "{{ value_json.hold }}";

        [JsonProperty("mode_cmd_t")] public string ModeCmdT => $"{_mqttPrefix}/climate/{_zoneId}/set/thermostatModeCmd";

        [JsonProperty("mode_stat_t")] public string ModeStatT => StateLocation;

        [JsonProperty("mode_stat_tpl")] public string ModeStatTpl => "{{ value_json.mode }}";

        [JsonProperty("temp_cmd_t")] public string TempCmdT => $"{_mqttPrefix}/climate/{_zoneId}/set/targetTempCmd";

        [JsonProperty("temp_stat_t")] public string TempStatT => StateLocation;

        [JsonProperty("temp_stat_tpl")] public string TempStatTpl => "{{ value_json.targetTemp }}";

        [JsonProperty("curr_temp_t")] public string CurrTempT => StateLocation;

        [JsonProperty("curr_temp_tpl")] public string CurrTempTpl => "{{ value_json.currentTemp }}";

        [JsonProperty("act_t")] public string ActT => StateLocation;

        [JsonProperty("act_tpl")] public string ActTpl => "{{ value_json.action }}";

        [JsonProperty("min_temp")] public string MinTemp => Constants.MinTemp.ToString(Constants.NumberFormat);

        [JsonProperty("max_temp")] public string MaxTemp =>  Constants.MaxTemp.ToString(Constants.NumberFormat);

        [JsonProperty("temp_step")] public string TempStep => Constants.TempStep.ToString(Constants.NumberFormat);

        [JsonProperty("modes")] public string[] Modes => new[] {Constants.ModeHeat};

        //[JsonProperty("icon")] public string Icon => Constants.Icon;

        [JsonProperty("device")]
        public Device Device { get; set; }

        [JsonProperty("hold_modes")]
        public string[] HoldModes => new[] {Constants.HoldModeTemporary, Constants.HoldModePermanent};
    }
}