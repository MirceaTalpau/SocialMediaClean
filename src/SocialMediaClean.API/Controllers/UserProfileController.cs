using LinkedFit.APPLICATION.Contracts;
using LinkedFit.APPLICATION.Helpers;
using LinkedFit.DOMAIN.Models.DTOs.UserProfile;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LinkedFit.API.Controllers
{
    [Route("api/v1/user-profile")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly ILogger<UserProfileController> _logger;
        private readonly IUserProfileService _userProfileService;

        public UserProfileController(ILogger<UserProfileController> logger, IUserProfileService userProfileService)
        {
            _logger = logger;
            _userProfileService = userProfileService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserProfile(int id)
        {
            try
            {
                string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var currentUserId = JwtHelper.GetUserIdFromToken(token);
                var userProfile = await _userProfileService.GetUserProfile(currentUserId,id);
                return Ok(userProfile);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting user profile");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error while getting user profile");
            }
        }
        [HttpPut]
        public async Task<IActionResult> UpdateUserProfile([FromForm]UserProfileDTO userProfileDTO)
        {
            try
            {
                string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var currentUserId = JwtHelper.GetUserIdFromToken(token);
                if(currentUserId != userProfileDTO.ID)
                {
                    return Unauthorized();
                }
                await _userProfileService.UpdateUserProfile(userProfileDTO);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating user profile");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error while updating user profile");
            }
        }
    }
}
