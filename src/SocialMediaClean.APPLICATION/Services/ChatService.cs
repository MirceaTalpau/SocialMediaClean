using LinkedFit.APPLICATION.Contracts;
using LinkedFit.DOMAIN.Models.Entities.Chats;
using LinkedFit.PERSISTANCE.Interfaces;

namespace LinkedFit.APPLICATION.Services
{
    public class ChatService : IChatService
    {
        private readonly IChatRepository _chatRepository;
        public ChatService(IChatRepository chatRepository)
        {
            _chatRepository = chatRepository;
        }

        public async Task<Chat> GetChatIdByUserIDAsync(int user1ID, int user2ID)
        {
            return await _chatRepository.GetChatIdByUserIDAsync(user1ID, user2ID);
        }
    }
}
