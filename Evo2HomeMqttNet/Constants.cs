using System;
using System.Collections.Generic;
using System.Text;

namespace Evo2HomeMqttNet
{
    public class Constants
    {
        public const string HoldModeTemporary = "Temporary";

        public const string HoldModePermanent = "Permanent";

        public const string HoldModeOff = "off";

        public const string ModeHeat = "heat";

        public const int MinTemp = 5;

        public const int MaxTemp = 35;

        public const double TempStep = 0.5;

        public const string NumberFormat = "F1";

        public const string Icon = "mdi:radiator";

        public const string Thermostat = "Thermostat";

        public const string ActionHeating = "heating";

        public const string ActionIdle = "idle";

        public const string Manufacturer = "HoneyWell";

        public const string Version = "Unknown";

        public const int DefaultMqttPort = 8113;

        public const string EvoClientName = "EvoHome";

        public const string TokenServiceClientName = "TokenService";
    }
}
