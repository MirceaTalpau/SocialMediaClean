using LinkedFit.DOMAIN.Models.Entities.Posts;
using LinkedFit.DOMAIN.Models.Views;

namespace LinkedFit.APPLICATION.Contracts
{
    public interface IFeedService
    {
        Task<IEnumerable<RecipePostView>> GetAllRecipePosts(int userId);
        Task<IEnumerable<NormalPostView>> GetAllPublicNormalPosts(int userId);
        Task<IEnumerable<ProgressPostView>> GetAllPublicProgressPosts(int userId);
        Task<IEnumerable<NormalPostView>> GetMyFriendsNormalPosts(int userId);
        Task<IEnumerable<RecipePostView>> GetMyFriendsRecipePosts(int userId);

    }
}