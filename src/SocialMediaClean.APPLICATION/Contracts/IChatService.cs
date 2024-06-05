using LinkedFit.DOMAIN.Models.DTOs.Chat;
using LinkedFit.DOMAIN.Models.Entities.Chats;

namespace LinkedFit.APPLICATION.Contracts
{
    public interface IChatService
    {
        Task<Chat> GetChatIdByUserIDAsync(int user1ID, int user2ID);
        Task StoreChatMessage(ChatDTO chat);
        Task<IEnumerable<ChatDTO>> GetChatsAsync(int chatId);
        Task<IEnumerable<ChatListDTO>> GetChatListDTOs(int userId);
    }
}