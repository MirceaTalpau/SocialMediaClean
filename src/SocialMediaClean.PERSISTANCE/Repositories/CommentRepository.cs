using Dapper;
using LinkedFit.DOMAIN.Models.DTOs.Comments;
using LinkedFit.PERSISTANCE.Interfaces;
using SocialMediaClean.INFRASTRUCTURE.Interfaces;
using System.Data;
using System.Transactions;

namespace LinkedFit.PERSISTANCE.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly IDbConnectionFactory _db;
        private readonly string ADD_COMMENT = "usp_Comment_Add_Comment";
        public CommentRepository(IDbConnectionFactory db)
        {
            _db = db;
        }
        public async Task CreateCommentAsync(AddCommentDTO comment)
        {
            using (var conn = await _db.CreateDbConnectionAsync())
            {
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        var parameters = new DynamicParameters();
                        parameters.Add("@PostID", comment.PostID);
                        parameters.Add("@AuthorID", comment.AuthorID);
                        if(comment.ParentID != 0)
                        {
                            parameters.Add("@ParentID", comment.ParentID);
                        }
                        parameters.Add("@Body", comment.Body);
                        await conn.QueryAsync(ADD_COMMENT, parameters, transaction, commandType: CommandType.StoredProcedure);
                        transaction.Commit();
                        return;
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
