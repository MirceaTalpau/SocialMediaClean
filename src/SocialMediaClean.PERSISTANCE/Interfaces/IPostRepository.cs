using LinkedFit.DOMAIN.Models.DTOs.Posts;

namespace LinkedFit.PERSISTANCE.Interfaces
{
    public interface IPostRepository
    {
        public Task<int> CreatePostNormalAsync(CreateNormalPostDTO post);
        public Task<int> CreatePostRecipeAsync(CreateRecipePostDTO post);

    }
}
