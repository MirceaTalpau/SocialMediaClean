using LinkedFit.DOMAIN.Models.DTOs.Posts;
using LinkedFit.DOMAIN.Models.Entities.Posts;
using LinkedFit.DOMAIN.Models.Views;

namespace LinkedFit.PERSISTANCE.Interfaces
{
    public interface IPostRepository
    {
        public Task<int> CreatePostNormalAsync(CreateNormalPostDTO post);
        public Task<int> CreatePostRecipeAsync(CreateRecipePostDTO post);
        public Task<int> CreatePostProgressAsync(CreateProgressPostDTO post);
        public Task<IEnumerable<NormalPostView>> GetAllNormalPostsAsync();
        public Task<IEnumerable<RecipePostView>> GetAllRecipePostsAsync();
        public Task<IEnumerable<MediaPostView>> GetMediaPostAsync(int postId);
        public Task<IEnumerable<Ingredient>> GetIngredientsAsync(int recipeId);
        public Task<IEnumerable<ProgressPostView>> GetPublicProgressPostsAsync();
        public Task<IEnumerable<MediaPostView>> DeletePostAsync(int postId);






    }
}
