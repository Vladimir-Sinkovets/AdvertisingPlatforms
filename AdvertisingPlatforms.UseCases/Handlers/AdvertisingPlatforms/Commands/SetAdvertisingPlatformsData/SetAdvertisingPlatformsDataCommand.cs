using MediatR;

namespace AdvertisingPlatforms.UseCases.Handlers.AdvertisingPlatforms.Commands.SetAdvertisingPlatformsData
{
    public class SetAdvertisingPlatformsDataCommand : IRequest<Result<SetAdvertisingPlatformsDataCommandResponse>>
    {
        public Stream Stream { get; set; }
    }
}
