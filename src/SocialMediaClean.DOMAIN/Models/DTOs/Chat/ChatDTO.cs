using LinkedFit.DOMAIN.Models.DTOs.Friends;
using System.Text.Json.Serialization;

namespace LinkedFit.DOMAIN.Models.DTOs.Chat
{
    public class ChatDTO
    {
        public Chat1DTO Sender { get; set; }
        public string ChatID { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
