using Microsoft.Extensions.DependencyInjection;

namespace AdvertisingPlatforms.UseCases
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddUseCases(this IServiceCollection services)
        {
            return services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
        }
    }
}
