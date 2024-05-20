using LinkedFit.APPLICATION.Contracts;
using LinkedFit.DOMAIN.Models.DTOs.Friends;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LinkedFit.API.Controllers
{
    [Route("api/v1/friends/")]
    [ApiController]
    public class FriendsController : ControllerBase
    {
        private readonly IFriendsService _friendsService;
        private readonly ILogger<FriendsController> _logger;
        public FriendsController(IFriendsService friendsService, ILogger<FriendsController> logger)
        {
            _friendsService = friendsService;
            _logger = logger;
        }

        [HttpPost("send-request")]
        public async Task<IActionResult> SendFriendRequest(FriendRequestDTO payload)
        {
            try
            {
                await _friendsService.SendFriendRequest(payload);
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpPost("accept-request")]
        public async Task<IActionResult> AcceptFriendRequest(FriendRequestDTO payload)
        {
            try
            {
                await _friendsService.AcceptFriendRequest(payload);
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
