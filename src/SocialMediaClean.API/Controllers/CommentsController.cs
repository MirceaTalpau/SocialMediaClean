using LinkedFit.APPLICATION.Contracts;
using LinkedFit.APPLICATION.Services;
using LinkedFit.DOMAIN.Models.DTOs.Comments;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LinkedFit.API.Controllers
{
    [Route("api/v1/comments/")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentsService _commentsService;
        private readonly ILogger<CommentsController> _logger;

        public CommentsController(ICommentsService commentsService, ILogger<CommentsController> logger)
        {
            _commentsService = commentsService;
            _logger = logger;
        }

        [HttpPost("add")]
        public async Task<IActionResult> CreateCommentAsync([FromBody] AddCommentDTO comment)
        {
            try
            {
                if(comment.Body == null || comment.Body == "")
                {
                    return BadRequest("Comment body cannot be empty");
                }
                var newComment = await _commentsService.CreateCommentAsync(comment);
                return Ok(newComment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating comment");
            }
        }
        [HttpGet("{postID}")]
        public async Task<IActionResult> GetCommentsAsync(int postID)
        {
            try
            {
                var comments = await _commentsService.GetCommentsAsync(postID);
                return Ok(comments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error getting comments");
            }
        }
        [HttpDelete("{commentID}")]
        public async Task<IActionResult> DeleteCommentAsync(int commentID)
        {
            try
            {
                await _commentsService.DeleteCommentAsync(commentID);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting comment");
            }
        }
    }
}
