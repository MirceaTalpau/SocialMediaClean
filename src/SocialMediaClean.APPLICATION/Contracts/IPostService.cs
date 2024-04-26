using LinkedFit.DOMAIN.Models.DTOs.Posts;

namespace LinkedFit.APPLICATION.Contracts
{
    public interface IPostService
    {
        public Task<int> CreatePostNormalAsync(CreateNormalPostDTO post);
        public Task<int> CreatePostRecipeAsync(CreateRecipePostDTO post);
        public Task<int> CreatePostProgressAsync(CreateProgressPostDTO post);
        public Task<bool> DeletePostAsync(int postId);


    }
}
