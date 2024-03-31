namespace LinkedFit.DOMAIN.Models.DTOs.Auth
{
    public class PasswordAndSalt
    {
        public int ID { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
    }
}
