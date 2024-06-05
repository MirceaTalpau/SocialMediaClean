using LinkedFit.DOMAIN.Models.DTOs.Chat;
using LinkedFit.DOMAIN.Models.Entities.Chats;
using LinkedFit.DOMAIN.Models.Views.Chats;

namespace LinkedFit.PERSISTANCE.Interfaces
{
    public interface IChatRepository
    {
        Task<Chat> GetChatIdByUserIDAsync(int user1ID, int user2ID);
        Task StoreChatMessage(ChatDTO chat);
        Task<IEnumerable<ChatViewDto>> GetChatsAsync(int chatId);
        Task<IEnumerable<ChatListDTO>> GetUserChats(int userId);

    }
}