using Dapper;
using LinkedFit.DOMAIN.Models.DTOs.Posts;
using LinkedFit.DOMAIN.Models.Entities.Posts;
using LinkedFit.DOMAIN.Models.Views;
using LinkedFit.PERSISTANCE.Interfaces;
using Microsoft.VisualBasic;
using SocialMediaClean.DOMAIN.Models.Entities;
using SocialMediaClean.INFRASTRUCTURE.Interfaces;
using System;
using System.Collections.Generic;
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

        private readonly string GET_ALL_NORMAL_POSTS = "usp_Post_GetNormalPosts";
        private readonly string GET_ALL_PUBLIC_NORMAL_POSTS = "usp_Post_GetPublicNormalPosts";
        private readonly string GET_ALL_RECIPE_POSTS = "usp_Post_GetAllRecipePosts";
        private readonly string GET_RECIPE_INGREDIENTS = "usp_Post_GetIngredients";
        private readonly string GET_MEDIA_POST = "usp_Post_GetMediaPost";
        private readonly string GET_PUBLIC_PROGRESS_POSTS = "usp_Post_GetPublicProgressPosts";

        private readonly string DELETE_POST = "usp_Post_DeletePost";



        public PostRepository(IDbConnectionFactory db)
        {
            _db = db;
        }
        
        private async Task<int> InsertMediaFilesAsync(CreateNormalPostDTO post,int postId, IDbConnection conn,IDbTransaction transaction)
        {
            try { 
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
            await conn.QueryAsync(INSERT_POST_MEDIA, mediaParameters, transaction, commandType: CommandType.StoredProcedure);
            return postId;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> TryInsertNormalPostAsync(CreateNormalPostDTO post,IDbConnection conn,IDbTransaction transaction)
        {
            try
            {
                var normalPostParameters = new DynamicParameters();
                normalPostParameters.Add("@AuthorID", post.AuthorID);
                normalPostParameters.Add("@Body", post.Body);
                normalPostParameters.Add("@StatusID", post.StatusID);
                normalPostParameters.Add("ID", dbType: DbType.Int32, direction: ParameterDirection.Output);
                await conn.QueryAsync(CREATE_POST_NORMAL, normalPostParameters, transaction, commandType: CommandType.StoredProcedure);
                int postId = normalPostParameters.Get<int>("ID");
                return postId;
            }
            catch (Exception)
            {
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
        //            await unitOfWork.Connection.QueryAsync(CREATE_POST_NORMAL, parameters, unitOfWork.Transaction, commandType: CommandType.StoredProcedure);
        //            int postId = parameters.Get<int>("ID");
        //            // If there are no pictures or videos, commit and return post ID
        //            postId = await InsertMediaFilesAsync(post, postId, unitOfWork);
        //            if(postId == 0)
        //            {
        //                unitOfWork.Rollback();
        //                return 0;
        //            }
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

        public async Task<int> CreatePostNormalAsync(CreateNormalPostDTO post)
        {
            using (var conn = await _db.CreateDbConnectionAsync())
            {
                try
                {
                    using( var transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            var parameters = new DynamicParameters();
                            parameters.Add("@AuthorID", post.AuthorID);
                            parameters.Add("@Body", post.Body);
                            parameters.Add("@StatusID", post.StatusID);
                            parameters.Add("ID", dbType: DbType.Int32, direction: ParameterDirection.Output);
                            await conn.QueryAsync(CREATE_POST_NORMAL, parameters, commandType: CommandType.StoredProcedure, transaction: transaction);
                            int postId = parameters.Get<int>("ID");

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
                            if (post.Videos == null && post.Pictures == null)
                            {
                                transaction.Commit();
                                return postId;
                            }
                            // Execute the media insertion stored procedure within the same transaction
                            await conn.QueryAsync(INSERT_POST_MEDIA, mediaParameters, commandType: CommandType.StoredProcedure, transaction: transaction);
                            transaction.Commit();
                            return postId; // Return the post ID
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
                    throw;
                }
                
            }
        }

        public async Task<int> CreatePostRecipeAsync(CreateRecipePostDTO post)
        {
            using (var conn = await _db.CreateDbConnectionAsync())
            {
                using(var transaction = conn.BeginTransaction())
                try
                {
                    var postId = await TryInsertNormalPostAsync(post,conn,transaction);
                    // If there are no pictures or videos, commit and return post ID
                    postId = await InsertMediaFilesAsync(post, postId, conn,transaction);
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
                    await conn.QueryAsync<int>(CREATE_POST_RECIPE, parameters, transaction, commandType: CommandType.StoredProcedure);
                    var recipeId = parameters.Get<int>("ID");
                    if(recipeId == 0)
                    {
                        transaction.Rollback();
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
                        await conn.QueryAsync(INSERT_RECIPE_INGREDIENTS, ingredientParameters, transaction, commandType: CommandType.StoredProcedure);
                        transaction.Commit();
                    }
                    return postId;
                }
                catch (Exception)
                {
                        transaction.Rollback();
                        throw;
                }
            }
        }

        public async Task<int> CreatePostProgressAsync(CreateProgressPostDTO post)
        {
            using (var conn = await _db.CreateDbConnectionAsync())
            {
                using(var transaction = conn.BeginTransaction())
                try
                {
                    //var userExists = await VerifyUserExistsAsync(post.AuthorID);
                    //if (userExists == 0)
                    //{
                    //    throw new Exception("User does not exist.");
                    //}
                    var postParameters = new DynamicParameters();
                    postParameters.Add("@AuthorID", post.AuthorID);
                    postParameters.Add("@Body", post.Body);
                    postParameters.Add("@StatusID", post.StatusID);
                    postParameters.Add("ID", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    await conn.QueryAsync(CREATE_POST_NORMAL, postParameters,transaction, commandType: CommandType.StoredProcedure);
                    int postId = postParameters.Get<int>("ID");
                    if (postId == 0)
                    {
                        transaction.Rollback();
                        return 0;
                    }
                   
                    var parameters = new DynamicParameters();
                    parameters.Add("@PostID", postId);
                    parameters.Add("@BeforeWeight", Int32.Parse(post.BeforeWeight));
                    parameters.Add("@AfterWeight", Int32.Parse(post.AfterWeight));
                    parameters.Add("@BeforePictureUri", post.BeforePictureUri);
                    parameters.Add("@AfterPictureUri", post.AfterPictureUri);
                    parameters.Add("@BeforeDate", DateTime.Parse(post.BeforeDate));
                    parameters.Add("@AfterDate", DateTime.Parse(post.AfterDate));
                    parameters.Add("ID", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    await conn.QueryAsync<int>(CREATE_POST_PROGRESS, parameters, transaction, commandType: CommandType.StoredProcedure);
                    var progressId = parameters.Get<int>("ID");
                    if (progressId == 0)
                    {
                        transaction.Rollback();
                        return 0;
                    }
                    else
                    {
                        transaction.Commit();
                    }
                    return postId;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
        public async Task<IEnumerable<NormalPostView>> GetAllNormalPostsAsync(int userId)
        {
            using (var _unitOfWork = await _db.CreateDbConnectionAsync())
            {
                try
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@CurrentUserID", userId);
                    IEnumerable<NormalPostView> posts = await _unitOfWork.QueryAsync<NormalPostView>(GET_ALL_PUBLIC_NORMAL_POSTS,parameters, commandType: CommandType.StoredProcedure);
                    if (posts == null)
                    {
                        throw new Exception();
                    }
                    return posts;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
        public async Task<IEnumerable<RecipePostView>> GetAllRecipePostsAsync(int userId)
        {
            using (var conn = await _db.CreateDbConnectionAsync())
            {
                try
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@CurrentUserID", userId);
                    IEnumerable<RecipePostView> posts = await conn.QueryAsync<RecipePostView>(GET_ALL_RECIPE_POSTS,parameters, commandType: CommandType.StoredProcedure);
                    if (posts == null)
                    {
                        throw new Exception();
                    }
                    return posts;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public async Task<IEnumerable<Ingredient>> GetIngredientsAsync(int recipeId)
        {
            using (var conn = await _db.CreateDbConnectionAsync())
            {
                try
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@RecipeID", recipeId);
                    IEnumerable<Ingredient> ingredients = await conn.QueryAsync<Ingredient>(GET_RECIPE_INGREDIENTS, parameters, commandType: CommandType.StoredProcedure);
                    if (ingredients == null)
                    {
                        throw new Exception();
                    }
                    return ingredients;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
        public async Task<IEnumerable<ProgressPostView>> GetPublicProgressPostsAsync(int userId)
        {
            using (var conn = await _db.CreateDbConnectionAsync())
            {
                try
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@CurrentUserID", userId);
                    IEnumerable<ProgressPostView> posts = await conn.QueryAsync<ProgressPostView>(GET_PUBLIC_PROGRESS_POSTS,parameters, commandType: CommandType.StoredProcedure);
                    if (posts == null)
                    {
                        throw new Exception();
                    }
                    return posts;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public async Task<IEnumerable<MediaPostView>> GetMediaPostAsync(int postId)
        {
            using (var conn = await _db.CreateDbConnectionAsync())
            {
                try
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@PostID", postId);
                    IEnumerable<MediaPostView> media = await conn.QueryAsync<MediaPostView>(GET_MEDIA_POST, parameters, commandType: CommandType.StoredProcedure);
                    if (media == null)
                    {
                        throw new Exception();
                    }
                    return media;
                }
                catch (Exception)
                {
                    throw;
                }
            }

        }

        public async Task<IEnumerable<MediaPostView>> DeletePostAsync(int postId)
        {
            using (var conn = await _db.CreateDbConnectionAsync())
            {
                try
                {
                    IEnumerable<MediaPostView> media = await GetMediaPostAsync(postId);
                    if(media == null)
                    {
                         media = new List<MediaPostView>();
                    }
                    var parameters = new DynamicParameters();
                    parameters.Add("@PostID", postId);
                    await conn.QueryAsync(DELETE_POST, parameters, commandType: CommandType.StoredProcedure);
                    return media;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }




}
