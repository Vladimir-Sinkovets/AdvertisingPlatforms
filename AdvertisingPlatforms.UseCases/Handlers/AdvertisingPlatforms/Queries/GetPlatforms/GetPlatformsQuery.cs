using MediatR;

namespace AdvertisingPlatforms.UseCases.Handlers.AdvertisingPlatforms.Queries.GetPlatforms
{
    public class GetPlatformsQuery : IRequest<Result<GetPlatformsQueryResponse>>
    {
        public string Location { get; set; }
    }
}
