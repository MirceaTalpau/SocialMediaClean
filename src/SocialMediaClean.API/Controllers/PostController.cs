using LinkedFit.APPLICATION.Contracts;
using LinkedFit.APPLICATION.Services;
using LinkedFit.DOMAIN.Models.DTOs.PostActions;
using LinkedFit.DOMAIN.Models.DTOs.Posts;
using LinkedFit.INFRASTRUCTURE.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMediaClean.API.Controllers;

namespace LinkedFit.API.Controllers
{
    [Route("api/post/")]
    [ApiController]
    [AllowAnonymous]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly IPostActionsService _postActionsService;
        private readonly ILogger<AuthController> _logger;
        private readonly IUploadFilesService _uploadFilesService;

        public PostController(IPostService postService, ILogger<AuthController> logger, IUploadFilesService uploadFilesService, IPostActionsService postActionsService)
        {
            _postService = postService;
            _logger = logger;
            _uploadFilesService = uploadFilesService;
            _postActionsService = postActionsService;
        }
        [HttpPost("normal")]
        public async Task<IActionResult> CreatePostNormalAsync([FromForm] CreateNormalPostDTO formData)
        {
            try
            {
                Console.WriteLine(formData);
                //var response = await _postService.CreatePostNormalAsync(post);
                var ceva =await _postService.CreatePostNormalAsync(formData);
                return Ok(ceva);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }
        [HttpPost("recipe")]
        public async Task<IActionResult> CreatePostRecipeAsync([FromForm] CreateRecipePostDTO formData)
        {
            try
            {
                Console.WriteLine(formData);
                //var response = await _postService.CreatePostRecipeAsync(post);
                var postId = await _postService.CreatePostRecipeAsync(formData);
                return Ok(formData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }
        [HttpPost("progress")]
        public async Task<IActionResult> CreatePostProgressAsync([FromForm] CreateProgressPostDTO formData)
        {
            try
            {
                Console.WriteLine(formData);
                //var response = await _postService.CreatePostProgressAsync(post);
                await _postService.CreatePostProgressAsync(formData);
                return Ok(formData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                throw;
            }
        }
        [HttpDelete("{postId}")]
        public async Task<IActionResult> DeleteNormalPostAsync(int postId)
        {
            try
            {
                var medias = await _postService.DeletePostAsync(postId);
                return Ok(medias);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("like")]
        public async Task<IActionResult> AddLikeAsync([FromBody] PostLikeDTO like)
        {
            try
            {
                await _postActionsService.AddLikeAsync(like);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
  
    }
}
