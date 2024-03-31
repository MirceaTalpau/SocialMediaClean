using Dapper;
using LinkedFit.DOMAIN.Models.DTOs.Posts;
using LinkedFit.PERSISTANCE.Interfaces;
using SocialMediaClean.INFRASTRUCTURE.Interfaces;
using System.Data;

namespace LinkedFit.PERSISTANCE.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly IDbConnectionFactory _db;
        private readonly string CREATE_POST_NORMAL = "usp_Post_CreateNormalPost";
        private readonly string INSERT_POST_MEDIA = "usp_Post_InsertPostMedia";

        public PostRepository(IDbConnectionFactory db)
        {
            _db = db;
        }
        public async Task<int> CreatePostNormalAsync(CreateNormalPostDTO post)
        {
            using (var connection = await _db.CreateDbConnectionAsync())
            {
                var PostID = 0;
                try { 
                var parameters = new DynamicParameters();
                parameters.Add("@AuthorID", post.AuthorID);
                parameters.Add("@Body", post.Body);
                parameters.Add("@StatusID", post.StatusID);
                parameters.Add("ID", dbType: DbType.Int32, direction: ParameterDirection.Output);
                await connection.QueryAsync<int>(CREATE_POST_NORMAL, parameters, commandType: CommandType.StoredProcedure);
                PostID = parameters.Get<int>("ID");

                using(var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var transactionParameters = new DynamicParameters();
                        DataTable dataTablePictures = new DataTable();
                        dataTablePictures.Columns.Add("PostID", typeof(int));
                        dataTablePictures.Columns.Add("PictureURI", typeof(string));
                        dataTablePictures.Columns.Add("CreatedAt", typeof(DateTime));

                        DataTable dataTableVideos = new DataTable();
                        dataTableVideos.Columns.Add("PostID", typeof(int));
                        dataTableVideos.Columns.Add("VideoURI", typeof(string));
                        dataTableVideos.Columns.Add("CreatedAt", typeof(DateTime));

                        foreach (var picture in post.Pictures)
                        {
                            dataTablePictures.Rows.Add(PostID, picture.PictureURI, picture.CreatedAt);
                        }

                        foreach (var video in post.Videos)
                        {
                            dataTableVideos.Rows.Add(PostID, video.VideoURI, video.CreatedAt);
                        }

                        transactionParameters.Add("@Pictures", dataTablePictures.AsTableValuedParameter("PicturesTableType"));
                        transactionParameters.Add("@Videos", dataTableVideos.AsTableValuedParameter("VideosTableType"));
                        await connection.QueryAsync(INSERT_POST_MEDIA, transactionParameters, transaction, commandType: CommandType.StoredProcedure);
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                    
                }
                }
                catch (Exception)
                {
                    return PostID;
                    throw;
                }
                return PostID;
            }
        }
    }
}
