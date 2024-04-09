using Dapper;
using LinkedFit.DOMAIN.Models.DTOs.Posts;
using LinkedFit.DOMAIN.Models.Entities;
using LinkedFit.PERSISTANCE.Interfaces;
using SocialMediaClean.INFRASTRUCTURE.Interfaces;
using System;
using System.Data;

namespace LinkedFit.PERSISTANCE.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly IDbConnectionFactory _db;
        private readonly IUnitOfWork _unitOfWork;
        private readonly string CREATE_POST_NORMAL = "usp_Post_CreateNormalPost";
        private readonly string INSERT_POST_MEDIA = "usp_Post_InsertPostMedia";
        private readonly string CREATE_POST_RECIPE = "usp_Post_CreateRecipePost";
        private readonly string INSERT_RECIPE_INGREDIENTS = "usp_Post_InsertRecipeIngredients";
        private readonly string CREATE_POST_PROGRESS = "usp_Post_CreateProgressPost";

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
        //public async Task<int> CreatePostNormalAsync(CreateNormalPostDTO post)
        //{
        //    var PostID = 0;
        //    try
        //    {
        //        var parameters = new DynamicParameters();
        //        parameters.Add("@AuthorID", post.AuthorID);
        //        parameters.Add("@Body", post.Body);
        //        parameters.Add("@StatusID", post.StatusID);
        //        parameters.Add("ID", dbType: DbType.Int32, direction: ParameterDirection.Output);
        //        await _unitOfWork.Connection.QueryAsync<int>(CREATE_POST_NORMAL, parameters, commandType: CommandType.StoredProcedure);
        //        PostID = parameters.Get<int>("ID");
        //        if (post.Pictures == null && post.Videos == null)
        //        {
        //            _unitOfWork.Commit();
        //            _unitOfWork.Dispose();
        //            return PostID;
        //        }
        //        var mediaParameters = new DynamicParameters();
        //        if (post.Pictures != null)
        //        {
        //            DataTable dataTablePictures = new DataTable();
        //            dataTablePictures.Columns.Add("PostID", typeof(int));
        //            dataTablePictures.Columns.Add("PictureURI", typeof(string));
        //            dataTablePictures.Columns.Add("CreatedAt", typeof(DateTime));
        //            foreach (var picture in post.Pictures)
        //            {
        //                dataTablePictures.Rows.Add(PostID, picture.PictureURI, picture.CreatedAt);
        //            }
        //            mediaParameters.Add("@Pictures", dataTablePictures.AsTableValuedParameter("PicturesTableType"));
        //        }
        //        if (post.Videos != null)
        //        {
        //            DataTable dataTableVideos = new DataTable();
        //            dataTableVideos.Columns.Add("PostID", typeof(int));
        //            dataTableVideos.Columns.Add("VideoURI", typeof(string));
        //            dataTableVideos.Columns.Add("CreatedAt", typeof(DateTime));
        //            foreach (var video in post.Videos)
        //            {
        //                dataTableVideos.Rows.Add(PostID, video.VideoURI, video.CreatedAt);
        //            }
        //            mediaParameters.Add("@Videos", dataTableVideos.AsTableValuedParameter("VideosTableType"));
        //        }
        //        await _unitOfWork.Connection.QueryAsync(INSERT_POST_MEDIA, mediaParameters, _unitOfWork.Transaction, commandType: CommandType.StoredProcedure);
        //        _unitOfWork.Commit();
        //        _unitOfWork.Dispose();
        //        return PostID;
        //    }
        //    catch (Exception)
        //    {
        //        _unitOfWork.Rollback();
        //        _unitOfWork.Dispose();
        //        throw;
        //    }

        //}

        private async Task<int> InsertMediaFilesAsync(CreateNormalPostDTO post,int postId, UnitOfWork unitOfWork)
        {
            if (post.Pictures == null && post.Videos == null)
            {
                return postId;
            }

            var mediaParameters = new DynamicParameters();

            // Process pictures if available
            if (post.Pictures != null)
            {
                DataTable dataTablePictures = new DataTable();
                dataTablePictures.Columns.Add("PostID", typeof(int));
                dataTablePictures.Columns.Add("PictureURI", typeof(string));
                dataTablePictures.Columns.Add("CreatedAt", typeof(DateTime));
                foreach (var picture in post.Pictures)
                {
                    dataTablePictures.Rows.Add(postId, picture.PictureURI, picture.CreatedAt);
                }
                mediaParameters.Add("@Pictures", dataTablePictures.AsTableValuedParameter("PicturesTableType"));
            }

            // Process videos if available
            if (post.Videos != null)
            {
                DataTable dataTableVideos = new DataTable();
                dataTableVideos.Columns.Add("PostID", typeof(int));
                dataTableVideos.Columns.Add("VideoURI", typeof(string));
                dataTableVideos.Columns.Add("CreatedAt", typeof(DateTime));
                foreach (var video in post.Videos)
                {
                    dataTableVideos.Rows.Add(postId, video.VideoURI, video.CreatedAt);
                }
                mediaParameters.Add("@Videos", dataTableVideos.AsTableValuedParameter("VideosTableType"));
            }

            // Execute the media insertion stored procedure within the same transaction
            await unitOfWork.Connection.QueryAsync(INSERT_POST_MEDIA, mediaParameters, unitOfWork.Transaction, commandType: CommandType.StoredProcedure);
            return postId;
        }

        public async Task<int> TryInsertNormalPostAsync(CreateNormalPostDTO post,UnitOfWork unitOfWork)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@AuthorID", post.AuthorID);
                parameters.Add("@Body", post.Body);
                parameters.Add("@StatusID", post.StatusID);
                parameters.Add("ID", dbType: DbType.Int32, direction: ParameterDirection.Output);
                await unitOfWork.Connection.QueryAsync<int>(CREATE_POST_NORMAL, parameters, unitOfWork.Transaction, commandType: CommandType.StoredProcedure);
                int postId = parameters.Get<int>("ID");
                return postId;
            }
            catch (Exception)
            {
                unitOfWork.Rollback();
                return 0;
                throw;
            }

        }

        public async Task<int> CreatePostNormalAsync(CreateNormalPostDTO post)
        {
            using (var unitOfWork = new UnitOfWork(_db))
            {
                try
                {
                    var postId = await TryInsertNormalPostAsync(post, unitOfWork);
                    // If there are no pictures or videos, commit and return post ID
                    postId = await InsertMediaFilesAsync(post, postId, unitOfWork);
                    unitOfWork.Commit(); // Commit the transaction
                    return postId; // Return the post ID
                }
                catch (Exception)
                {
                    unitOfWork.Rollback(); // Rollback transaction in case of exception
                    return 0;
                    throw;
                }
            }


        }

        public async Task<int> CreatePostRecipeAsync(CreateRecipePostDTO post)
        {
            using (var _unitOfWork = new UnitOfWork(_db))
            {
                try
                {
                    var postId = await TryInsertNormalPostAsync(post, _unitOfWork);
                    // If there are no pictures or videos, commit and return post ID
                    postId = await InsertMediaFilesAsync(post, postId, _unitOfWork);
                    var parameters = new DynamicParameters();
                    parameters.Add("@PostID", postId);
                    parameters.Add("@Name", post.Name);
                    parameters.Add("@Description", post.Description);
                    parameters.Add("@Instructions", post.Instructions);
                    parameters.Add("@CookingTime", Int32.Parse(post.CookingTime));
                    parameters.Add("@Servings", Int32.Parse(post.Servings));
                    parameters.Add("@Calories", Int32.Parse(post.Calories));
                    parameters.Add("@Protein", Int32.Parse(post.Protein));
                    parameters.Add("@Carbs", Int32.Parse(post.Carbs));
                    parameters.Add("@Fat", Int32.Parse(post.Fat));
                    parameters.Add("ID", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    await _unitOfWork.Connection.QueryAsync<int>(CREATE_POST_RECIPE, parameters, _unitOfWork.Transaction, commandType: CommandType.StoredProcedure);
                    var recipeId = parameters.Get<int>("ID");
                    if(recipeId == 0)
                    {
                        _unitOfWork.Rollback();
                        return 0;
                    }
                    else
                    {
                        // Process ingredients
                        DataTable dataTableIngredients = new DataTable();
                        dataTableIngredients.Columns.Add("RecipeID", typeof(int));
                        dataTableIngredients.Columns.Add("Name", typeof(string));
                        dataTableIngredients.Columns.Add("Quantity", typeof(int));
                        dataTableIngredients.Columns.Add("Unit", typeof(string));
                        foreach (var ingredient in post.Ingredients)
                        {
                            dataTableIngredients.Rows.Add(recipeId, ingredient.Name, Int32.Parse(ingredient.Quantity), ingredient.Unit);
                        }
                        var ingredientParameters = new DynamicParameters();
                        ingredientParameters.Add("@Ingredients", dataTableIngredients.AsTableValuedParameter("IngredientsTableType"));
                        await _unitOfWork.Connection.QueryAsync(INSERT_RECIPE_INGREDIENTS, ingredientParameters, _unitOfWork.Transaction, commandType: CommandType.StoredProcedure);
                        _unitOfWork.Commit();
                    }
                    return postId;
                }
                catch (Exception)
                {
                    _unitOfWork.Rollback();
                    return 0;
                    throw;
                }
            }
        }

        public async Task<int> CreatePostProgressAsync(CreateProgressPostDTO post)
        {
            using (var _unitOfWork = new UnitOfWork(_db))
            {
                try
                {
                    var postId = await TryInsertNormalPostAsync(post, _unitOfWork);
                    // If there are no pictures or videos, commit and return post ID
                    //MODIFICA AICI
                    //postId = await InsertMediaFilesAsync(post, postId, _unitOfWork);
                    var parameters = new DynamicParameters();
                    parameters.Add("@PostID", postId);
                    parameters.Add("@BeforeWeight", Int32.Parse(post.BeforeWeight));
                    parameters.Add("@AfterWeight", Int32.Parse(post.AfterWeight));
                    parameters.Add("@BeforePictureUri", post.BeforePictureUri);
                    parameters.Add("@AfterPictureUri", post.AfterPictureUri);
                    parameters.Add("@BeforeDate", DateTime.Parse(post.BeforeDate));
                    parameters.Add("@AfterDate", DateTime.Parse(post.AfterDate));
                    parameters.Add("ID", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    await _unitOfWork.Connection.QueryAsync<int>(CREATE_POST_PROGRESS, parameters, _unitOfWork.Transaction, commandType: CommandType.StoredProcedure);
                    var progressId = parameters.Get<int>("ID");
                    if (progressId == 0)
                    {
                        _unitOfWork.Rollback();
                        return 0;
                    }
                    else
                    {
                        _unitOfWork.Commit();
                    }
                    return postId;
                }
                catch (Exception)
                {
                    _unitOfWork.Rollback();
                    return 0;
                    throw;
                }
            }
        }

    }


}
