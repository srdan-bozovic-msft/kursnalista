using System;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using MSC.Universal.Shared.Contracts.DeviceServices;
using MSC.Universal.Shared.Contracts.Services;

namespace MSC.Universal.Shared.Implementation
{
    public class LocationService : ILocationService
    {
        public async Task<ServiceResult<Geoposition>> GetGeopositionAsync(uint? accuracyInMeters = 50, int maximumAgeInMinutes = 5, int timeoutInSeconds = 10)
        {
            var geolocator = new Geolocator { DesiredAccuracyInMeters = accuracyInMeters };

            try
            {
                return await geolocator.GetGeopositionAsync(
                    TimeSpan.FromMinutes(maximumAgeInMinutes),
                    TimeSpan.FromSeconds(timeoutInSeconds)
                    ).AsTask().ConfigureAwait(false);
            }
            catch (Exception xcp)
            {
                if ((uint)xcp.HResult == 0x80004004)
                {
                    // the application does not have the right capability or the location master switch is off
                    return ServiceResult<Geoposition>.Create(
                        null, 
                        null, 
                        false, 
                        401,
                        "the application does not have the right capability or the location master switch is off");
                }
                return ServiceResult<Geoposition>.CreateError(xcp);
            }
        }
    }
}
