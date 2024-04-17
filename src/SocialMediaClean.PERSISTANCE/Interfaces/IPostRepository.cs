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
        public Task<IEnumerable<NormalPostView>> GetAllNormalPosts();
        public Task<IEnumerable<RecipePostView>> GetAllRecipePosts();


    }
}
