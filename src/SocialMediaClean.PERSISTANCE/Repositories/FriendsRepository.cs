using Dapper;
using LinkedFit.DOMAIN.Models.DTOs.Friends;
using LinkedFit.PERSISTANCE.Interfaces;
using SocialMediaClean.INFRASTRUCTURE.Interfaces;
using System.Data;

namespace LinkedFit.PERSISTANCE.Repositories
{
    public class FriendsRepository : IFriendsRepository
    {
        private readonly IDbConnectionFactory _db;

        private readonly string SEND_FRIEND_REQUEST = "usp_Friends_SendFriendRequest";
        private readonly string ACCEPT_FRIEND_REQUEST = "usp_Friends_AcceptFriendRequest";
        public FriendsRepository(IDbConnectionFactory db)
        {
            _db = db;
        }

        public async Task SendFriendRequest(FriendRequestDTO payload)
        {
            using (var conn = await _db.CreateDbConnectionAsync())
            {
                try
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@SenderID", payload.SenderID);
                    parameters.Add("@ReceiverID", payload.ReceiverID);
                    await conn.QueryAsync(SEND_FRIEND_REQUEST, parameters, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }

            }
        }
        public async Task AcceptFriendRequest(FriendRequestDTO payload)
        {
            using (var conn = await _db.CreateDbConnectionAsync())
            {
                try
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@SenderID", payload.SenderID);
                    parameters.Add("@ReceiverID", payload.ReceiverID);
                    await conn.QueryAsync(ACCEPT_FRIEND_REQUEST, parameters, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }

            }
        }
    }
}
