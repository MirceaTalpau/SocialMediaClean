using LinkedFit.DOMAIN.Models.Entities.Posts;
using LinkedFit.DOMAIN.Models.Views;

namespace LinkedFit.APPLICATION.Contracts
{
    public interface IFeedService
    {
        Task<IEnumerable<RecipePostView>> GetAllRecipePosts();
        Task<IEnumerable<NormalPostView>> GetAllPublicNormalPosts();
    }
}