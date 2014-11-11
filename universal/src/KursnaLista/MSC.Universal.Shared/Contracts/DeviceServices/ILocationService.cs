using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using MSC.Universal.Shared.Contracts.Services;

namespace MSC.Universal.Shared.Contracts.DeviceServices
{
    public interface ILocationService
    {
        Task<ServiceResult<Geoposition>> GetGeopositionAsync(uint? accuracyInMeters = 50, int maximumAgeInMinutes = 5, int timeoutInSeconds = 10);
    }
}
