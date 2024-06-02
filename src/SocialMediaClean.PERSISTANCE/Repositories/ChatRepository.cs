using Dapper;
using LinkedFit.DOMAIN.Models.Entities.Chats;
using LinkedFit.PERSISTANCE.Interfaces;
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
    }
}
