namespace LinkedFit.DOMAIN.Models.Views.Chats
{
    public class ChatViewDto
    {
        public int ChatID { get; set; }
        public int AuthorID { get; set; }
        public string AuthorFirstName { get; set; }
        public string AuthorLastName { get; set; }
        public string AuthorProfilePictureURL { get; set; }
        public string Body { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
