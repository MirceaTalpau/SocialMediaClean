using LinkedFit.APPLICATION.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LinkedFit.API.Controllers
{
    [Route("api/v1/chat")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;
        private ILogger<ChatController> _logger;
        public ChatController(IChatService chatService, ILogger<ChatController> logger)
        {
            _logger = logger;
            _chatService = chatService;
        }

        [HttpGet]
        public async Task<IActionResult> GetChatIdByUserIDAsync([FromQuery]int user1ID,[FromQuery] int user2ID)
        {
            try
            {
                _logger.LogInformation($"GetChatIdByUserIDAsync called,User1: {user1ID}, User2:{user2ID}");
                var chat = await _chatService.GetChatIdByUserIDAsync(user1ID, user2ID);
                return Ok(chat);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in GetChatIdByUserIDAsync");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error in GetChatIdByUserIDAsync");
            }
        }
    }
}
