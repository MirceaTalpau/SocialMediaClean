using LinkedFit.APPLICATION.Contracts;
using LinkedFit.APPLICATION.Helpers;
using LinkedFit.DOMAIN.Models.Entities.Posts;
using LinkedFit.DOMAIN.Models.Views;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMediaClean.API.Controllers;
using System.IdentityModel.Tokens.Jwt;

namespace LinkedFit.API.Controllers
{ 
    [Route("api/v1/feed/")]
    [AllowAnonymous]
    [Authorize]
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
            try
            {
                string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var userId = JwtHelper.GetUserIdFromToken(token);
                var posts = await _feedService.GetAllPublicNormalPosts(userId);
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
                string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var userId = JwtHelper.GetUserIdFromToken(token);
                var posts = await _feedService.GetAllRecipePosts(userId);
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
                string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var userId = JwtHelper.GetUserIdFromToken(token);
                var posts = await _feedService.GetAllPublicProgressPosts(userId);
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
