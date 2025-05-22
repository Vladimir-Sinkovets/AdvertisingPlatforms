using AdvertisingPlatforms.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace AdvertisingPlatforms.Controllers
{
    public class BaseController : Controller
    {
        public IActionResult SendResult<T>(Result<T> result) where T : class
        {
            return result.Status switch
            {
                Status.Success => Ok(result),
                Status.BadData => BadRequest(result),
                _ => StatusCode(500, result),
            };
        }
    }
}
