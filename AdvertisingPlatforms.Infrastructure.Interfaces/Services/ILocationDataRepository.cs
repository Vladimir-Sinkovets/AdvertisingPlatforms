using AdvertisingPlatforms.Entities.Models;

namespace AdvertisingPlatforms.Infrastructure.Interfaces.Services
{
    public interface ILocationDataRepository
    {
        IEnumerable<LocationData> LocationData { get; set; }
    }
}
