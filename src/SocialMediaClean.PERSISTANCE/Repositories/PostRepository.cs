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
        private readonly IUnitOfWork _unitOfWork;
        private readonly string CREATE_POST_NORMAL = "usp_Post_CreateNormalPost";
        private readonly string INSERT_POST_MEDIA = "usp_Post_InsertPostMedia";

        public PostRepository(IDbConnectionFactory db)
        {
            _db = db;
            _unitOfWork = new UnitOfWork(_db);
        }
        //    public async Task<int> CreatePostNormalAsync(CreateNormalPostDTO post)
        //    {
        //        using (var connection = await _db.CreateDbConnectionAsync())
        //        {
        //            var PostID = 0;
        //            try { 
        //            var parameters = new DynamicParameters();
        //            parameters.Add("@AuthorID", post.AuthorID);
        //            parameters.Add("@Body", post.Body);
        //            parameters.Add("@StatusID", post.StatusID);
        //            parameters.Add("ID", dbType: DbType.Int32, direction: ParameterDirection.Output);
        //            await connection.QueryAsync<int>(CREATE_POST_NORMAL, parameters, commandType: CommandType.StoredProcedure);
        //            PostID = parameters.Get<int>("ID");
        //            if(post.Pictures == null && post.Videos == null)
        //                {
        //                return PostID;
        //            }
        //            using(var transaction = connection.BeginTransaction())
        //            {
        //                try
        //                {
        //                    var transactionParameters = new DynamicParameters();
        //                    if(post.Pictures != null)
        //                    {
        //                        DataTable dataTablePictures = new DataTable();
        //                        dataTablePictures.Columns.Add("PostID", typeof(int));
        //                        dataTablePictures.Columns.Add("PictureURI", typeof(string));
        //                        dataTablePictures.Columns.Add("CreatedAt", typeof(DateTime));
        //                        foreach (var picture in post.Pictures)
        //                        {
        //                            dataTablePictures.Rows.Add(PostID, picture.PictureURI, picture.CreatedAt);
        //                        }
        //                        transactionParameters.Add("@Pictures", dataTablePictures.AsTableValuedParameter("PicturesTableType"));
        //                        }

        //                    if (post.Videos != null)
        //                    {
        //                        DataTable dataTableVideos = new DataTable();
        //                        dataTableVideos.Columns.Add("PostID", typeof(int));
        //                        dataTableVideos.Columns.Add("VideoURI", typeof(string));
        //                        dataTableVideos.Columns.Add("CreatedAt", typeof(DateTime));
        //                        foreach (var video in post.Videos)
        //                        {
        //                        dataTableVideos.Rows.Add(PostID, video.VideoURI, video.CreatedAt);
        //                        }
        //                        transactionParameters.Add("@Videos", dataTableVideos.AsTableValuedParameter("VideosTableType"));

        //                    }


        //                        await connection.QueryAsync(INSERT_POST_MEDIA, transactionParameters, transaction, commandType: CommandType.StoredProcedure);
        //                    transaction.Commit();
        //                }
        //                catch (Exception)
        //                {
        //                    transaction.Rollback();
        //                    throw;
        //                }

        //            }
        //            }
        //            catch (Exception)
        //            {
        //                return PostID;
        //                throw;
        //            }
        //            return PostID;
        //        }
        //    }
        //}
        public async Task<int> CreatePostNormalAsync(CreateNormalPostDTO post)
        {
            var PostID = 0;
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@AuthorID", post.AuthorID);
                parameters.Add("@Body", post.Body);
                parameters.Add("@StatusID", post.StatusID);
                parameters.Add("ID", dbType: DbType.Int32, direction: ParameterDirection.Output);
                await _unitOfWork.Connection.QueryAsync<int>(CREATE_POST_NORMAL, parameters, commandType: CommandType.StoredProcedure);
                PostID = parameters.Get<int>("ID");
                if (post.Pictures == null && post.Videos == null)
                {
                    _unitOfWork.Commit();
                    _unitOfWork.Dispose();
                    return PostID;
                }
                var mediaParameters = new DynamicParameters();
                if (post.Pictures != null)
                {
                    DataTable dataTablePictures = new DataTable();
                    dataTablePictures.Columns.Add("PostID", typeof(int));
                    dataTablePictures.Columns.Add("PictureURI", typeof(string));
                    dataTablePictures.Columns.Add("CreatedAt", typeof(DateTime));
                    foreach (var picture in post.Pictures)
                    {
                        dataTablePictures.Rows.Add(PostID, picture.PictureURI, picture.CreatedAt);
                    }
                    mediaParameters.Add("@Pictures", dataTablePictures.AsTableValuedParameter("PicturesTableType"));
                }
                if (post.Videos != null)
                {
                    DataTable dataTableVideos = new DataTable();
                    dataTableVideos.Columns.Add("PostID", typeof(int));
                    dataTableVideos.Columns.Add("VideoURI", typeof(string));
                    dataTableVideos.Columns.Add("CreatedAt", typeof(DateTime));
                    foreach (var video in post.Videos)
                    {
                        dataTableVideos.Rows.Add(PostID, video.VideoURI, video.CreatedAt);
                    }
                    mediaParameters.Add("@Videos", dataTableVideos.AsTableValuedParameter("VideosTableType"));
                }
                await _unitOfWork.Connection.QueryAsync(INSERT_POST_MEDIA, mediaParameters, _unitOfWork.Transaction, commandType: CommandType.StoredProcedure);
                _unitOfWork.Commit();
                _unitOfWork.Dispose();
                return PostID;
            }
            catch (Exception)
            {
                _unitOfWork.Rollback();
                _unitOfWork.Dispose();
                throw;
            }

        }

        //public async Task<int> CreatePostNormalAsync(CreateNormalPostDTO post)
        //{
        //    using (var unitOfWork = new UnitOfWork(_db))
        //    {
        //        try
        //        {
        //            var parameters = new DynamicParameters();
        //            parameters.Add("@AuthorID", post.AuthorID);
        //            parameters.Add("@Body", post.Body);
        //            parameters.Add("@StatusID", post.StatusID);
        //            parameters.Add("ID", dbType: DbType.Int32, direction: ParameterDirection.Output);
        //            await unitOfWork.Connection.QueryAsync<int>(CREATE_POST_NORMAL, parameters, unitOfWork.Transaction, commandType: CommandType.StoredProcedure);
        //            int postId = parameters.Get<int>("ID");

        //            // If there are no pictures or videos, commit and return post ID
        //            if (post.Pictures == null && post.Videos == null)
        //            {
        //                unitOfWork.Commit();
        //                return postId;
        //            }

        //            var mediaParameters = new DynamicParameters();

        //            // Process pictures if available
        //            if (post.Pictures != null)
        //            {
        //                DataTable dataTablePictures = new DataTable();
        //                dataTablePictures.Columns.Add("PostID", typeof(int));
        //                dataTablePictures.Columns.Add("PictureURI", typeof(string));
        //                dataTablePictures.Columns.Add("CreatedAt", typeof(DateTime));
        //                foreach (var picture in post.Pictures)
        //                {
        //                    dataTablePictures.Rows.Add(postId, picture.PictureURI, picture.CreatedAt);
        //                }
        //                mediaParameters.Add("@Pictures", dataTablePictures.AsTableValuedParameter("PicturesTableType"));
        //            }

        //            // Process videos if available
        //            if (post.Videos != null)
        //            {
        //                DataTable dataTableVideos = new DataTable();
        //                dataTableVideos.Columns.Add("PostID", typeof(int));
        //                dataTableVideos.Columns.Add("VideoURI", typeof(string));
        //                dataTableVideos.Columns.Add("CreatedAt", typeof(DateTime));
        //                foreach (var video in post.Videos)
        //                {
        //                    dataTableVideos.Rows.Add(postId, video.VideoURI, video.CreatedAt);
        //                }
        //                mediaParameters.Add("@Videos", dataTableVideos.AsTableValuedParameter("VideosTableType"));
        //            }

        //            // Execute the media insertion stored procedure within the same transaction
        //            await unitOfWork.Connection.QueryAsync(INSERT_POST_MEDIA, mediaParameters, unitOfWork.Transaction, commandType: CommandType.StoredProcedure);
        //            unitOfWork.Commit(); // Commit the transaction
        //            return postId; // Return the post ID
        //        }
        //        catch (Exception)
        //        {
        //            unitOfWork.Rollback(); // Rollback transaction in case of exception
        //            throw;
        //        }
        //    }


        //}

    }


}
