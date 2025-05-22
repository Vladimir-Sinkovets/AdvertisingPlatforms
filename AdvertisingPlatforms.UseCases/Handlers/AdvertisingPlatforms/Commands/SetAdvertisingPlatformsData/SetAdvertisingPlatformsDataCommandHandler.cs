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

                    if (parts.Length != 2 )
                        throw new FormatException();

                    var locationData = new LocationData()
                    {
                        Title = parts[0].Trim(),
                        Locations = parts[1].Split(',')
                            .Select(x => new LocationSegments()
                            {
                                Segments = x.Split("/", StringSplitOptions.RemoveEmptyEntries)
                            }),
                    };


                    locations.Add(locationData);
                }
            }
            catch
            {
                return Result<SetAdvertisingPlatformsDataCommandResponse>.Create(Status.ServerError, "Server error");
            }

            dataRepository.LocationData = locations;

            return Result<SetAdvertisingPlatformsDataCommandResponse>.Create(Status.Success, "Success");
        }
    }
}
