using SocialMediaClean.DOMAIN;

namespace SocialMediaClean.DOMAIN.Models.Entities
{
    public class User
    {
        public int ID { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime JoinedAt { get; set; }
        public DateTime BirthDay { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string ProfilePictureURL { get; set; }
        public string CoverPictureURL { get; set; }
    }
}
