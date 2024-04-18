using Dapper;
using LinkedFit.DOMAIN.Models.DTOs.Posts;
using LinkedFit.DOMAIN.Models.Entities.Posts;
using LinkedFit.DOMAIN.Models.Views;
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

        private readonly string GET_ALL_NORMAL_POSTS = "usp_Post_GetNormalPosts";
        private readonly string GET_ALL_PUBLIC_NORMAL_POSTS = "usp_Post_GetPublicNormalPosts";
        private readonly string GET_ALL_RECIPE_POSTS = "usp_Post_GetAllRecipePosts";
        private readonly string GET_MEDIA_POST = "usp_Post_GetMediaPost";


        public PostRepository(IDbConnectionFactory db)
        {
            _db = db;
            _unitOfWork = new UnitOfWork(_db);
        }
        
        private async Task<int> InsertMediaFilesAsync(CreateNormalPostDTO post,int postId, UnitOfWork unitOfWork)
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
            await unitOfWork.Connection.QueryAsync(INSERT_POST_MEDIA, mediaParameters, unitOfWork.Transaction, commandType: CommandType.StoredProcedure);
            return postId;
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }

        public async Task<int> TryInsertNormalPostAsync(CreateNormalPostDTO post)
        {
            try
            {
                using (var conn = await _db.CreateDbConnectionAsync() )
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@AuthorID", post.AuthorID);
                    parameters.Add("@Body", post.Body);
                    parameters.Add("@StatusID", post.StatusID);
                    parameters.Add("ID", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    await conn.QueryAsync(CREATE_POST_NORMAL, parameters, commandType: CommandType.StoredProcedure);
                    int postId = parameters.Get<int>("ID");
                    return postId;
                }
                
            }
            catch (Exception ex)
            {
                
                return 0;
                throw ex;
            }

        }

        public async Task<int> CreatePostNormalAsync(CreateNormalPostDTO post)
        {
            using (var unitOfWork = new UnitOfWork(_db))
            {
                try
                {
                    var postId = await TryInsertNormalPostAsync(post);
                    // If there are no pictures or videos, commit and return post ID
                    postId = await InsertMediaFilesAsync(post, postId, unitOfWork);
                    if(postId == 0)
                    {
                        unitOfWork.Rollback();
                        return 0;
                    }
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
                    var postId = await TryInsertNormalPostAsync(post);
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
                    //var userExists = await VerifyUserExistsAsync(post.AuthorID);
                    //if (userExists == 0)
                    //{
                    //    throw new Exception("User does not exist.");
                    //}
                    var postId = await TryInsertNormalPostAsync(post);
                    if (postId == 0)
                    {
                        _unitOfWork.Rollback();
                        return 0;
                    }
                    // If there are no pictures or videos, commit and return post ID
                    //MODIFICA AICI
                    postId = await InsertMediaFilesAsync(post, postId, _unitOfWork);
                    if (postId == 0)
                    {
                        _unitOfWork.Rollback();
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
                    throw;
                }
            }
        }
        public async Task<IEnumerable<NormalPostView>> GetAllNormalPosts()
        {
            using (var _unitOfWork = await _db.CreateDbConnectionAsync())
            {
                try
                {
                    IEnumerable<NormalPostView> posts = await _unitOfWork.QueryAsync<NormalPostView>(GET_ALL_PUBLIC_NORMAL_POSTS, commandType: CommandType.StoredProcedure);
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
        public async Task<IEnumerable<RecipePostView>> GetAllRecipePosts()
        {
            using (var conn = await _db.CreateDbConnectionAsync())
            {
                try
                {
                    IEnumerable<RecipePostView> posts = await conn.QueryAsync<RecipePostView>(GET_ALL_RECIPE_POSTS, commandType: CommandType.StoredProcedure);
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

        public async Task<IEnumerable<MediaPostView>> GetMediaPost(int postId)
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
    }




}
