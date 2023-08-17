namespace SocialMediaClean.DOMAIN.Models.DTOs
{
    public class PasswordAndSalt
    {
        public int ID { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
    }
}
