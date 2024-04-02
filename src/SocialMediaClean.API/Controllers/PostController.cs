using LinkedFit.APPLICATION.Contracts;
using LinkedFit.APPLICATION.Services;
using LinkedFit.DOMAIN.Models.DTOs.Posts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMediaClean.API.Controllers;

namespace LinkedFit.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly ILogger<AuthController> _logger;

        public PostController(IPostService postService, ILogger<AuthController> logger)
        {
            _postService = postService;
            _logger = logger;
        }
        [HttpPost]
        public async Task<IActionResult> CreatePostNormalAsync([FromForm] CreateNormalPostDTO formData)
        {
            try
            {

                Console.WriteLine(DateTime.Parse(formData.CreatedAt));
                //var response = await _postService.CreatePostNormalAsync(post);
                return Ok(DateTime.Parse(formData.CreatedAt));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }
    }
}
