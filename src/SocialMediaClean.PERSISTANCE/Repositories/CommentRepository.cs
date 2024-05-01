using Dapper;
using LinkedFit.DOMAIN.Models.DTOs.Comments;
using LinkedFit.DOMAIN.Models.Views.Comments;
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
        private readonly string GET_COMMENTS = "usp_Comment_Get_Comments";
        private readonly string GET_COMMENT = "usp_Comment_Get_Comment";
        private readonly string DELETE_COMMENT = "usp_Comment_Delete_Comment";
        public CommentRepository(IDbConnectionFactory db)
        {
            _db = db;
        }
        public async Task<int> CreateCommentAsync(AddCommentDTO comment)
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
                        if (comment.ParentID != -1)
                        {
                            parameters.Add("@ParentID", comment.ParentID);
                        }
                        parameters.Add("@Body", comment.Body);
                        parameters.Add("@InsertedID", dbType: DbType.Int32, direction: ParameterDirection.Output);
                        await conn.QueryAsync(ADD_COMMENT, parameters, transaction, commandType: CommandType.StoredProcedure);
                        var insertedID = parameters.Get<int>("@InsertedID");
                        transaction.Commit();
                        return insertedID;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
        public async Task<CommentView> GetCommentAsync(int commentID)
        {
            using (var conn = await _db.CreateDbConnectionAsync())
            {
                try
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@CommentID", commentID);
                    return await conn.QueryFirstOrDefaultAsync<CommentView>(GET_COMMENT, parameters, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
        public async Task<IEnumerable<CommentView>> GetCommentsAsync(int postID)
        {
            using (var conn = await _db.CreateDbConnectionAsync())
            {
                try
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@PostID", postID);
                    return await conn.QueryAsync<CommentView>(GET_COMMENTS, parameters, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
        public async Task DeleteCommentAsync(int commentID)
        {
            using (var conn = await _db.CreateDbConnectionAsync())
            {
                try
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@CommentID", commentID);
                    await conn.ExecuteAsync(DELETE_COMMENT, parameters, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }

            }
        }
    }
}
