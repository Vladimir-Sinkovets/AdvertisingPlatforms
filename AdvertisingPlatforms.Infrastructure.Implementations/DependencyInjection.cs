using AdvertisingPlatforms.Infrastructure.Implementations.Services;
using AdvertisingPlatforms.Infrastructure.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AdvertisingPlatforms.Infrastructure.Implementations
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services.AddSingleton<ILocationDataRepository, LocationDataRepository>();
        }
    }
}
