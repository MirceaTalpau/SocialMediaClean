using SocialMediaClean.APPLICATION.DTOs;
using SocialMediaClean.DOMAIN.Models.DTOs;

namespace SocialMediaClean.PERSISTANCE.Interfaces
{
    public interface IAuthRepository
    {
        Task<bool> RegisterUserAsync(RegisterRequestDTO user,string confirmationToken);
        Task<PasswordAndSalt> GetPasswordAndSaltAsync(string email);
        Task<bool> IsEmailVerifiedAsync(string email);


    }
}
