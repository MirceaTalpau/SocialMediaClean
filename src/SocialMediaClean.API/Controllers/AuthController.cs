using LinkedFit.DOMAIN.Models.DTOs.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SocialMediaClean.APPLICATION.Contracts;
using SocialMediaClean.APPLICATION.Requests;
using SocialMediaClean.APPLICATION.Response;

namespace SocialMediaClean.API.Controllers
{
    [Route("api/v1/auth/")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;
        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;

        }

        //TO DO MODIFY PROCEDURE FOR CHECKING EXISTING USER
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> LoginUserAsync([FromBody]LoginRequest request)
        {
            var requestJson = JsonConvert.SerializeObject(request, Formatting.Indented);
            _logger.LogInformation($"Login request received!");
            try
            { 
            var response = await _authService.LoginUserAsync(request);
            if(response.Token == null)
            {
                _logger.LogWarning($"Login request: {requestJson} failed with the response message: {response.Message}",requestJson,response.Message);
                return Unauthorized(response.Message);
            }
            return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception has been raised at the payload {requestJson} with the exception message: {ex.Message}");
                throw;
            }
        }

        
        [HttpPost("register")]
        public async Task<ActionResult<RegisterResponse>> RegisterUserAsync([FromBody]RegisterRequest request)
        {
            var requestJson = JsonConvert.SerializeObject(request, Formatting.Indented);
            _logger.LogInformation("Register request received");
            try
            { 
            var response = await _authService.RegisterUserAsync(request);
            if (!response.Success)
            {
                _logger.LogWarning($"Register user failed: {requestJson}, message: {response.Message}");
                return BadRequest(response);
            }
            return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception raised at the payload : {requestJson} with the exception message: {ex.Message}");
                throw;
            }
        }

        [HttpPost("login-google")]
        public async Task<ActionResult<LoginResponse>> LoginGoogleAsync([FromBody]RegisterRequestDTO request)
        {
            var requestJson = JsonConvert.SerializeObject(request, Formatting.Indented);
            _logger.LogInformation($"Login google request received!");
            try
            {
            var response = await _authService.LogOrRegisterGmailUserAsync(request);
            if (response.Token == null)
                {
                _logger.LogWarning($"Login google request: {requestJson} failed with the response message: {response.Message}", requestJson, response.Message);
                return Unauthorized(response.Message);
            }
            return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception has been raised at the payload {requestJson} with the exception message: {ex.Message}");
                throw;
            }
        }
        [Authorize]
        [HttpGet("validate-jwt")]
        public async Task<ActionResult<bool>> IsJwtValid()
        {
            var accessToken = HttpContext.Request.Headers["accessToken"];
            var response = await _authService.IsJwtValid(accessToken.ToString());
            if (response==false)
            {
                return Unauthorized(response);
            }
            return Ok(response);
        }

    }
}
