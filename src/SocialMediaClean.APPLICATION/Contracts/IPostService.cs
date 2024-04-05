using LinkedFit.DOMAIN.Models.DTOs.Posts;

namespace LinkedFit.APPLICATION.Contracts
{
    public interface IPostService
    {
        public Task<int> CreatePostNormalAsync(CreateNormalPostDTO post);
        public Task<int> CreatePostRecipeAsync(CreateRecipePostDTO post);

    }
}
