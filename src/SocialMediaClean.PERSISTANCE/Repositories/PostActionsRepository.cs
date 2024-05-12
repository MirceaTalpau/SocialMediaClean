using Dapper;
using LinkedFit.DOMAIN.Models.DTOs.PostActions;
using LinkedFit.PERSISTANCE.Interfaces;
using SocialMediaClean.INFRASTRUCTURE.Interfaces;

namespace LinkedFit.PERSISTANCE.Repositories
{
    public class PostActionsRepository : IPostActionsRepository
    {
        private readonly IDbConnectionFactory _db;
        private readonly string POST_ADD_LIKE = "usp_Post_AddLike";
        public PostActionsRepository(IDbConnectionFactory db)
        {
            _db = db;
        }

        public async Task AddLikeAsync(PostLikeDTO like)
        {
            using (var conn = _db.CreateDbConnection())
            {
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        var parameters = new DynamicParameters();
                        parameters.Add("@PostID", like.PostID);
                        parameters.Add("@UserID", like.UserID);
                        await conn.QueryAsync(POST_ADD_LIKE, parameters, transaction, commandType: System.Data.CommandType.StoredProcedure);
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public async Task SharePostAsync(int postId,int userId)
        {
            using (var conn = await _db.CreateDbConnectionAsync())
            {
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        var parameters = new DynamicParameters();
                        parameters.Add("@PostID", postId);
                        parameters.Add("@UserID", userId);
                        await conn.QueryAsync("usp_Post_Share", parameters, transaction, commandType: System.Data.CommandType.StoredProcedure);
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

    }
}
