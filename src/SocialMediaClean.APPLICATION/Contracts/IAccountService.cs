using LinkedFit.DOMAIN.Models.DTOs.Account;
using LinkedFit.DOMAIN.Models.DTOs.Auth;
using SocialMediaClean.APPLICATION.Response;

namespace SocialMediaClean.APPLICATION.Contracts
{
    public interface IAccountService
    {
        Task<BaseResponse> SendPasswordResetMailAsync(string email);
        bool VerifyPasswordResetHmacCode(String codeBase64Url, out Int32 userId);
        Task ChangePassword(ChangePasswordDTO passwordDTO, Int32 userID);
        Task<BaseResponse> ResendEmailConfirmationTokenAsync(string email);
        Task<BaseResponse> VerifyEmailAsync(string email, string token);
        Task<ResetTokenUserIdDTO> VerifyResetPasswordTokenAsync(string token);
        Task<UserDataDTO> GetUserDataDTOAsync(int id);




    }
}
