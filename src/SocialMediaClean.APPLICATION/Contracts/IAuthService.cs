using LinkedFit.DOMAIN.Models.DTOs.Auth;
using SocialMediaClean.APPLICATION.Requests;
using SocialMediaClean.APPLICATION.Response;

namespace SocialMediaClean.APPLICATION.Contracts
{
    public interface IAuthService
    {
        Task<LoginResponse> LoginUserAsync(LoginRequest loginRequest);
        Task<RegisterResponse> RegisterUserAsync(RegisterRequest registerRequest);
        Task<LoginResponse> LogOrRegisterGmailUserAsync(RegisterRequestDTO req);

    }
}
