using SocialMediaClean.APPLICATION.Response;
using SocialMediaClean.DOMAIN.Models.DTOs;

namespace SocialMediaClean.APPLICATION.Contracts
{
    public interface IAccountService
    {
        Task<BaseResponse> SendPasswordResetMailAsync(string email);
        bool VerifyPasswordResetHmacCode(String codeBase64Url, out Int32 userId);
        Task ChangePassword(ChangePasswordDTO passwordDTO, Int32 userID);
        Task<BaseResponse> ResendEmailConfirmationTokenAsync(string email);
        Task<BaseResponse> VerifyEmailAsync(string email, string token);


    }
}
