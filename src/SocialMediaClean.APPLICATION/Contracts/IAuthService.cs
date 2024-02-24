using SocialMediaClean.APPLICATION.DTOs;
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
