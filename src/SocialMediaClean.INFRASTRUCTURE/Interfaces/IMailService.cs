using SocialMediaClean.INFRASTRUCTURE.Models;

namespace SocialMediaClean.INFRASTRUCTURE.Interfaces
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
    }
}
