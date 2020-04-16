# Total Comfort Connect Evo Home 2 Mqtt Bridge for Europe

## Features

Polls the evohome api for changes (as specified in poll duration)
Cache to help limit number of calls
Mqqt discovery for home assistant

### Prerequisities

Requires an Mqtt Server to point to and Home Assistant for automatic discovery to work.

#### Environment Variables

* "evoFileLocation": Location of output files and cache (required)
* "evoUsername": Evohome username
* "evoPassword": Evohome passowrd,
* "evoLocationName": Evo home location name "Home" for example,
* "evoPollDuration": Poll duration is seconds, I recommend setting this to 180,
* "mqttPrefix": The mqtt topic prefix "evohome" for example,
* "mqttUser": Mqtt server username,
* "mqttPassword": Mqtt server password,
* "mqttConnection": Ip address of the Mqtt service,
* "mqttPort": Mqtt Port (Usually "1883"),
* "mqttClientName": Client name used when connecting to the Mqtt Service eg. "Evo2MqttNet",
* "disableMqtt": "true" or "false". Will turn off mqtt including setting home assistant discovery,
* "mqqtDiscovery": "true" or "false". If set to true will publish discovery in home assistant to automatically configure heating zones
* "mqqtDiscoveryPrefix": Should be set to "homeassistant" to work correctly.
