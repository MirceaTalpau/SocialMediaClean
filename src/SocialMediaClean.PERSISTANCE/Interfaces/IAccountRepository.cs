using LinkedFit.DOMAIN.Models.DTOs.Account;
using LinkedFit.DOMAIN.Models.DTOs.Auth;

namespace SocialMediaClean.PERSISTANCE.Interfaces
{
    public interface IAccountRepository
    {
        Task<string> CheckExistingUserAsync(string email,string? phone);
        Task ChangePasswordAsync(IDPasswordSaltDTO changePassword);
        Task InsertForgotPasswordTokenAsync(string token, int ID);
        Task UpdateEmailConfirmationTokenAsync(string token, int ID);
        Task<string> GetEmailConfirmationTokenAsync(string email);
        Task<string> GetForgotPasswordToken(string email);
        Task ValidateEmailAsync(string email);
        Task<bool> IsEmailVerifiedAsync(string email);
        Task<string> GetPasswordResetTokenAsync(int id);
        Task<UserDataDTO> GetUserDataAsync(int id);

    }
}
