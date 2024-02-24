using SocialMediaClean.DOMAIN;

namespace SocialMediaClean.APPLICATION.DTOs
{
    public class RegisterRequestDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; } = null;
        public string? Password { get; set; }
        public string? PasswordSalt { get; set; }
        public DateTime? BirthDay { get; set; }
        public string? Gender { get; set; }
        public string? ProfileAvatar { get; set; }
    }
}
