using LinkedFit.DOMAIN.Models.Views;

namespace LinkedFit.DOMAIN.Models.DTOs.UserProfile
{
    public class UserProfileDTO
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; } = "";
        public string ProfilePictureURL { get; set; }
        public string Bio { get; set; } = "";
        public DateTime JoinedAt { get; set; }
        public DateTime BirthDay { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Address { get; set; } = "";
        public IEnumerable<NormalPostView>? Posts { get; set; }
        public IFormFile? ProfilePicture { get; set; }
    }
}
