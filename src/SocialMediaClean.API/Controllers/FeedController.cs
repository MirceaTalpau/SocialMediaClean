using LinkedFit.APPLICATION.Contracts;
using LinkedFit.DOMAIN.Models.Entities.Posts;
using LinkedFit.DOMAIN.Models.Views;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LinkedFit.API.Controllers
{ 
    [Route("api/v1/feed/")]
    [AllowAnonymous]
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
        [HttpGet("normal")]
        public async Task<IActionResult> GetAllPublicNormalPosts()
        {
            try{
            var posts = await _feedService.GetAllPublicNormalPosts();
            return Ok(posts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }

        [HttpGet("recipe")]
        public async Task<IActionResult> GetAllPublicRecipeFeed()
        {
            try
            {
                var posts = await _feedService.GetAllRecipePosts();
                return Ok(posts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }
        [HttpGet("progress")]
        public async Task<IActionResult> GetAllPublicProgressPosts()
        {
            try
            {
                var posts = await _feedService.GetAllPublicProgressPosts();
                return Ok(posts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }
    }
}
