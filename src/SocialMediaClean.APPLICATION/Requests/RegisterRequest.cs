using SocialMediaClean.DOMAIN.Enums;

namespace SocialMediaClean.APPLICATION.Requests
{
    public class RegisterRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; } = null;
        public string Password { get; set; }
        public DateTime BirthDay { get; set; }
        public string Gender { get; set; }
        
    }
}
