using Dapper;
using LinkedFit.DOMAIN.Models.Views;
using LinkedFit.PERSISTANCE.Interfaces;
using SocialMediaClean.INFRASTRUCTURE.Interfaces;
using System.Data;

namespace LinkedFit.PERSISTANCE.Repositories
{
    public class FeedRepository : IFeedRepository
    {
        private readonly IDbConnectionFactory _db;
        public FeedRepository(IDbConnectionFactory db)
        {
            _db = db;
        }

        public async Task<IEnumerable<NormalPostView>> GetMyFriendsPosts(int userId)
        {
            using (var conn = await _db.CreateDbConnectionAsync())
            {
                try
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@UserID", userId);
                    var posts = await conn.QueryAsync<NormalPostView>("usp_Feed_GetMyFriendsNormalPosts", parameters, commandType: CommandType.StoredProcedure);
                    return posts;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
        public async Task<IEnumerable<RecipePostView>> GetMyFriendsRecipePost(int userId)
        {
            using(var conn = await _db.CreateDbConnectionAsync())
            {
                try
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@CurrentUserID", userId);
                    var posts = await conn.QueryAsync<RecipePostView>("usp_Feed_GetMyFriendsRecipePosts", parameters, commandType: CommandType.StoredProcedure);
                    return posts;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
        public async Task<IEnumerable<ProgressPostView>> GetMyFriendsProgressPost(int userId)
        {             
            using (var conn = await _db.CreateDbConnectionAsync())
            {
                try
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@CurrentUserID", userId);
                    var posts = await conn.QueryAsync<ProgressPostView>("usp_Feed_GetMyFriendsProgressPosts", parameters, commandType: CommandType.StoredProcedure);
                    return posts;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
