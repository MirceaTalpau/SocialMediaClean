using Dapper;
using LinkedFit.DOMAIN.Models.DTOs.Chat;
using LinkedFit.DOMAIN.Models.Entities.Chats;
using LinkedFit.DOMAIN.Models.Views.Chats;
using LinkedFit.PERSISTANCE.Interfaces;
using SocialMediaClean.DOMAIN.Models.Entities;
using SocialMediaClean.INFRASTRUCTURE.Interfaces;
using System.Data;

namespace LinkedFit.PERSISTANCE.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private readonly IDbConnectionFactory _db;

        private readonly string GET_CHAT_ID = "usp_Chat_GetChatIdByUserID";
        private readonly string INSERT_CHAT = "usp_Chat_InsertChat";
        public ChatRepository(IDbConnectionFactory db)
        {
            _db = db;
        }

        public async Task<Chat> GetChatIdByUserIDAsync(int user1ID, int user2ID)
        {
            try
            {
                using (var conn = await _db.CreateDbConnectionAsync())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@User1ID", user1ID);
                    parameters.Add("@User2ID", user2ID);
                    var chat = await conn.QueryFirstOrDefaultAsync<Chat>(GET_CHAT_ID, parameters, commandType: CommandType.StoredProcedure);
                    if (chat == null)
                    {
                        await conn.QueryAsync(INSERT_CHAT, parameters, commandType: CommandType.StoredProcedure);
                        chat = await conn.QueryFirstOrDefaultAsync<Chat>(GET_CHAT_ID, parameters, commandType: CommandType.StoredProcedure);
                        return chat;
                    }
                    return chat;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task StoreChatMessage(ChatDTO chat)
        {
            try
            {
                using (var conn = await _db.CreateDbConnectionAsync())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@ChatID", Int32.Parse(chat.ChatID));
                    parameters.Add("@AuthorID", chat.Sender.ID);
                    parameters.Add("@Message", chat.Message);
                    parameters.Add("@CreatedAt", chat.CreatedAt);
                    await conn.ExecuteAsync("usp_Chat_InsertChatMessage", parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task <IEnumerable<ChatViewDto>> GetChatsAsync(int chatId)
        {
            using (var conn = await _db.CreateDbConnectionAsync())
            {
                var query = @"
    SELECT
        cm.ID AS ChatID,
        cm.AuthorID,
        u.FirstName AS AuthorFirstName,
        u.LastName AS AuthorLastName,
        u.ProfilePictureURL AS AuthorProfilePictureURL,
        cm.Body,
        cm.CreatedAt
    FROM
        ChatMessages cm
        INNER JOIN Users u ON cm.AuthorID = u.ID
    WHERE
        cm.ChatID = @ChatId
    ORDER BY
        cm.CreatedAt DESC";

                var chatMessages = await conn.QueryAsync<ChatViewDto>(query, new { ChatId = chatId });
                return chatMessages;
            }
        }

        public async Task<IEnumerable<ChatListDTO>> GetUserChats(int userId)
        {
            using (var conn = await _db.CreateDbConnectionAsync())
            {
                try
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@UserID", userId);
                    var chats = await conn.QueryAsync<ChatListDTO>("usp_Chat_GetUserChats", parameters, commandType: CommandType.StoredProcedure);
                    return chats;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
