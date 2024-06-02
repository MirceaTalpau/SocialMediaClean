namespace LinkedFit.DOMAIN.Models.Entities.Chats
{
    public class Chat
    {
        public int ID { get; set; }
        public int User1ID { get; set; }
        public int User2ID { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
