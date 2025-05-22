using AdvertisingPlatforms.UseCases.Handlers.AdvertisingPlatforms.Commands.SetAdvertisingPlatformsData;
using AdvertisingPlatforms.UseCases.Handlers.AdvertisingPlatforms.Queries.GetPlatforms;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AdvertisingPlatforms.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlatformsController(IMediator mediatr) : BaseController
    {
        [HttpPost]
        [Route("upload")]
        public async Task<IActionResult> UploadAdvertisingPlatformsData(IFormFile file)
        {
            var command = new SetAdvertisingPlatformsDataCommand()
            {
                Stream = file.OpenReadStream(),
            };

            var result = await mediatr.Send(command);

            return SendResult(result);
        }

        [HttpGet]
        [Route("getplatforms")]
        public async Task<IActionResult> GetPlatforms(string location)
        {
            var query = new GetPlatformsQuery()
            {
                Location = location,
            };

            var result = await mediatr.Send(query);

            return SendResult(result);
        }
    }
}
