using AdvertisingPlatforms.Entities.Models;
using AdvertisingPlatforms.Infrastructure.Interfaces.Services;

namespace AdvertisingPlatforms.Infrastructure.Implementations.Services
{
    public class LocationDataRepository : ILocationDataRepository
    {
        public IEnumerable<LocationData> LocationData { get; set; } = Enumerable.Empty<LocationData>();
    }
}
