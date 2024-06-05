namespace LinkedFit.DOMAIN.Models.DTOs.Chat
{
    public class ChatListDTO
    {
        public int ChatID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfilePictureURL { get; set; }
        public string LastMessage { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
