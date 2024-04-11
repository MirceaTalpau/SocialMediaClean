using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LinkedFit.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetFeed()
        {
            return Ok("Feed");
        }
    }
}
