using LinkedFit.APPLICATION.Contracts;
using LinkedFit.DOMAIN.Models.Views;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LinkedFit.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedController : ControllerBase
    {
        private readonly IFeedService _feedService;
        private ILogger<FeedController> _logger;
        public FeedController(IFeedService feedService, ILogger<FeedController> logger)
        {
            _feedService = feedService;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> GetRecipeFeed()
        {
            try
            {
                IEnumerable<RecipePostView> posts = await _feedService.GetAllRecipePosts();
                var post = posts.FirstOrDefault();
                return Ok(post);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }
    }
}
