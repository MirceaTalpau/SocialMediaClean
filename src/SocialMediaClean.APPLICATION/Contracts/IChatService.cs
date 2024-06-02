using LinkedFit.DOMAIN.Models.Entities.Chats;

namespace LinkedFit.APPLICATION.Contracts
{
    public interface IChatService
    {
        Task<Chat> GetChatIdByUserIDAsync(int user1ID, int user2ID);
    }
}