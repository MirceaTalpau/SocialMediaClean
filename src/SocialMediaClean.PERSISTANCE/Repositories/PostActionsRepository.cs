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
        private readonly string POST_SHARE = "usp_Post_Share";
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

        public async Task SharePostAsync(PostShareDTO share)
        {
            using (var conn = await _db.CreateDbConnectionAsync())
            {
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        var parameters = new DynamicParameters();
                        parameters.Add("@PostID", share.PostID);
                        parameters.Add("@UserID", share.UserID);
                        await conn.QueryAsync(POST_SHARE, parameters, transaction, commandType: System.Data.CommandType.StoredProcedure);
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
