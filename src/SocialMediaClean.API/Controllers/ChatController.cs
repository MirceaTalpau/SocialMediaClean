using LinkedFit.APPLICATION.Contracts;
using LinkedFit.DOMAIN.Models.DTOs.Chat;
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

        [HttpPost]
        public async Task<IActionResult> StoreChatMessage([FromBody] ChatDTO chat)
        {
            try
            {
                _logger.LogInformation($"StoreChatMessage called, Chat: {chat}");
                await _chatService.StoreChatMessage(chat);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in StoreChatMessage");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error in StoreChatMessage");
            }
        }

        [HttpGet("{chatId}")]
        public async Task<IActionResult> GetChatsAsync(int chatId)
        {
            try
            {
                _logger.LogInformation($"GetChatsAsync called, UserId: {chatId}");
                var chats = await _chatService.GetChatsAsync(chatId);
                return Ok(chats);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetChatsAsync");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error in GetChatsAsync");
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetChatListDTOs(int userId)
        {
            try
            {
                _logger.LogInformation($"GetChatListDTOs called, UserId: {userId}");
                var chatListDTOs = await _chatService.GetChatListDTOs(userId);
                return Ok(chatListDTOs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetChatListDTOs");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error in GetChatListDTOs");
            }
        }


    }
}
