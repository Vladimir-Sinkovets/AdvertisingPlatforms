using AdvertisingPlatforms.UseCases.Handlers.AdvertisingPlatforms.Commands.SetAdvertisingPlatformsData;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AdvertisingPlatforms.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlatformsController(IMediator mediatr) : ControllerBase
    {
        [HttpPost]
        [Route("upload")]
        public async Task<IActionResult> UploadAdvertisingPlatformsData(IFormFile file)
        {
            var command = new SetAdvertisingPlatformsDataCommand()
            {
                Stream = file.OpenReadStream(),
            };

            await mediatr.Send(command);

            return Ok();
        }
    }
}
