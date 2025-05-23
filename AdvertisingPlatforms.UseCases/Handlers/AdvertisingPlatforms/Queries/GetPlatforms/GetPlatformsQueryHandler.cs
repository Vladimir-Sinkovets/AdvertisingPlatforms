using AdvertisingPlatforms.Entities.Models;
using AdvertisingPlatforms.Infrastructure.Interfaces.Services;
using MediatR;

namespace AdvertisingPlatforms.UseCases.Handlers.AdvertisingPlatforms.Queries.GetPlatforms
{
    public class GetPlatformsQueryHandler(ILocationDataRepository dataRepository) : IRequestHandler<GetPlatformsQuery, Result<GetPlatformsQueryResponse>>
    {
        public async Task<Result<GetPlatformsQueryResponse>> Handle(GetPlatformsQuery request, CancellationToken cancellationToken)
        {
            var requestLocationSegments = NormalizeLocation(request.Location);

            if (requestLocationSegments.Length == 0)
                return Result<GetPlatformsQueryResponse>.Create(Status.BadData, "Wrong location format");

            var platforms = dataRepository.LocationData
                .Where(x => x.Locations.Any(x => IsMatch(x.Segments, requestLocationSegments)))
                .Select(x => x.Title)
                .ToList();

            return Result<GetPlatformsQueryResponse>.Create(Status.Success, "Success", 
                new GetPlatformsQueryResponse()
                {
                    Platforms = platforms,
                });
        }

        private static bool IsMatch(string[] segments, string[] requestLocationSegments)
        {
            if (segments.Length > requestLocationSegments.Length) return false;

            for (int i = 0; i < segments.Length; i++)
            {
                if (segments[i] != requestLocationSegments[i])
                {
                    return false;
                }
            }

            return true;
        }

        private static string[] NormalizeLocation(string location)
        {
            return location.Trim()
                .ToLowerInvariant()
                .Split('/', StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
