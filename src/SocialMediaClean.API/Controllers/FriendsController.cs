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
                _logger.LogInformation("Sending friend request");
                await _friendsService.SendFriendRequest(payload);
                _logger.LogInformation($"Friend request sent {payload}",payload);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error sending friend request {ex.Message}",ex.Message);
                throw;
            }
        }
        [HttpPost("accept-request")]
        public async Task<IActionResult> AcceptFriendRequest(FriendRequestDTO payload)
        {
            try
            {
                _logger.LogInformation("Accepting friend request");
                await _friendsService.AcceptFriendRequest(payload);
                _logger.LogInformation($"Friend request accepted {payload}",payload);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error accepting friend request {ex.Message}",ex.Message);
                throw;
            }
        }
        [HttpGet("{userID}")]
        public async Task<IActionResult> GetMyFriends(int userID)
        {
            try
            {
                _logger.LogInformation("Getting friends");
                var friends = await _friendsService.GetMyFriendsAsync(userID);
                _logger.LogInformation($"Friends retrieved {friends}",friends);
                return Ok(friends);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting friends {ex.Message}",ex.Message);
                throw;
            }
        }
        [HttpGet("get-friend-requests/{userID}")]
        public async Task<IActionResult> GetFriendRequests(int userID)
        {
            try
            {
                _logger.LogInformation("Getting friend requests");
                var friendRequests = await _friendsService.GetMyFriendRequests(userID);
                _logger.LogInformation($"Friend requests retrieved {friendRequests}",friendRequests);
                return Ok(friendRequests);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting friend requests {ex.Message}",ex.Message);
                throw;
            }
        }
        [HttpDelete("remove-friend")]
        public async Task<IActionResult> RemoveFriend(FriendRequestDTO payload)
        {
            try
            {
                _logger.LogInformation("Removing friend");
                await _friendsService.RemoveFriend(payload);
                _logger.LogInformation($"Friend removed {payload}",payload);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error removing friend {ex.Message}",ex.Message);
                throw;
            }
        }
        [HttpDelete("remove-friend-request")]
        public async Task<IActionResult> RemoveFriendRequest(FriendRequestDTO payload)
        {
            try
            {
                _logger.LogInformation("Removing friend request");
                await _friendsService.RemoveFriendRequest(payload);
                _logger.LogInformation($"Friend request removed {payload}",payload);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error removing friend request {ex.Message}",ex.Message);
                throw;
            }
        }

    }
}
