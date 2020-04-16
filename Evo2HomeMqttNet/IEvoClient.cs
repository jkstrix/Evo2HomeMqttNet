using System.Collections.Generic;
using System.Threading.Tasks;
using Evo2HomeMqttNet.Models.Location;
using Evo2HomeMqttNet.Models.Status;
using Evo2HomeMqttNet.Models.User;
using RestEase;

namespace Evo2HomeMqttNet
{
    public interface IEvoClient
    {
        [Header("Authorization")]
        string AuthHeader { get; set;}

        [Get("emea/api/v1/userAccount")]
        Task<UserAccount> GetUserAccountAsync();

        [Get("emea/api/v1/location/installationInfo")]
        Task<IEnumerable<Location>> GetLocationAsync([Query("userId")] string userId,
                [Query("includeTemperatureControlSystems")] bool includeTemperatureControlSystems);

        [Get("emea/api/v1/location/{locationId}/status")]
        Task<LocationStatus> GetLocationStatusAsync([Path]string locationId, [Query("includeTemperatureControlSystems")]bool includeTemperatureControlSystems);

        [Put("emea/api/v1/temperatureZone/{zoneId}/heatSetpoint")]
        Task HeatSeatPointAsync([Path] string zoneId, [Body] SetCommand setCommand);
    }
}

