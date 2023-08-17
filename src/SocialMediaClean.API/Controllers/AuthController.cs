using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialMediaClean.APPLICATION.Contracts;
using SocialMediaClean.APPLICATION.Requests;
using SocialMediaClean.APPLICATION.Response;

namespace SocialMediaClean.API.Controllers
{
    [Route("api/v1/auth")]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [Route("/login")]
        [HttpPost]
        public async Task<ActionResult<LoginResponse>> LoginUserAsync(LoginRequest request)
        {
            var response = await _authService.LoginUserAsync(request);
            if(response.Token.Equals(String.Empty))
            {
                return Unauthorized(response);
            }
            return Ok(response);
        }

        [Route("/register")]
        [HttpPost]
        public async Task<ActionResult<RegisterResponse>> RegisterUserAsync(RegisterRequest request)
        {
            try { 
            var response = await _authService.RegisterUserAsync(request);
            if (!response.Succes)
            {
                return BadRequest(response);
            }
            return Ok(response);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
