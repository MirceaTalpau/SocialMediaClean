using LinkedFit.APPLICATION.Contracts;
using LinkedFit.DOMAIN.Models.DTOs.Chat;
using LinkedFit.DOMAIN.Models.Entities.Chats;
using LinkedFit.DOMAIN.Models.Views.Chats;
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
        public async Task StoreChatMessage(ChatDTO chat)
        {
            await _chatRepository.StoreChatMessage(chat);
        }
        public async Task<IEnumerable<ChatDTO>> GetChatsAsync(int chatId)
        {
            var chatViewDtos =  await _chatRepository.GetChatsAsync(chatId);

            var chatDtos = chatViewDtos.Select(chatViewDto => new ChatDTO
            {
                ChatID = chatViewDto.ChatID.ToString(),
                Sender = new Chat1DTO
                {
                    ID = chatViewDto.AuthorID,
                    FirstName = chatViewDto.AuthorFirstName,
                    LastName = chatViewDto.AuthorLastName,
                    ProfilePictureURL = chatViewDto.AuthorProfilePictureURL
                },
                Message = chatViewDto.Body,
                CreatedAt = chatViewDto.CreatedAt
            });
            return chatDtos;
        }
        public async Task<IEnumerable<ChatListDTO>> GetChatListDTOs(int userId)
        {
            return await _chatRepository.GetUserChats(userId);
        }
    }
}
