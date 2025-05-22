using AdvertisingPlatforms.Entities.Models;
using AdvertisingPlatforms.Infrastructure.Interfaces.Services;
using MediatR;

namespace AdvertisingPlatforms.UseCases.Handlers.AdvertisingPlatforms.Commands.SetAdvertisingPlatformsData
{
    public class SetAdvertisingPlatformsDataCommandHandler(ILocationDataRepository dataRepository)
        : IRequestHandler<SetAdvertisingPlatformsDataCommand, Result<SetAdvertisingPlatformsDataCommandResponse>>
    {
        public async Task<Result<SetAdvertisingPlatformsDataCommandResponse>> Handle(SetAdvertisingPlatformsDataCommand request, CancellationToken cancellationToken)
        {
            var locations = new List<LocationData>();
            
            try
            {
                using var streamReader = new StreamReader(request.Stream);

                var line = string.Empty;

                while ((line = await streamReader.ReadLineAsync(cancellationToken)) != null)
                {
                    var parts = line.Split(':');

                    var locationData = new LocationData()
                    {
                        Title = parts[0].Trim(),
                        Locations = parts[1].Split(','),
                    };

                    if (IsValidLocation(locationData))
                    {
                        throw new FormatException();
                    }

                    locations.Add(locationData);
                }
            }
            catch
            {
                return Result<SetAdvertisingPlatformsDataCommandResponse>.Create(Status.BadData, "Wrong file format");
            }

            dataRepository.LocationData = locations;

            return Result<SetAdvertisingPlatformsDataCommandResponse>.Create(Status.Success, "Success");
        }

        private static bool IsValidLocation(LocationData locationData)
        {
            if (string.IsNullOrWhiteSpace(locationData.Title))
                return false;

            foreach (var location in locationData.Locations)
            {
                if (!location.StartsWith('/')) return false;

                var segments = location.Split('/');

                if (segments.All(s => s.All(char.IsLetter)))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
