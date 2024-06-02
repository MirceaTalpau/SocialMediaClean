using LinkedFit.DOMAIN.Models.Entities.Chats;

namespace LinkedFit.PERSISTANCE.Interfaces
{
    public interface IChatRepository
    {
        Task<Chat> GetChatIdByUserIDAsync(int user1ID, int user2ID);
    }
}