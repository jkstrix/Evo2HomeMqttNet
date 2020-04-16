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

## Docker - Details can be found at:
https://hub.docker.com/r/jkstrix/evo2homemqttnet

## License

MIT License

Evo2HomeMqttNet Copyright (c) 2020 James Knight

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
