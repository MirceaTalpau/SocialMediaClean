using LinkedFit.DOMAIN.Models.Views;

namespace LinkedFit.PERSISTANCE.Interfaces
{
    public interface IFeedRepository
    {
        Task<IEnumerable<NormalPostView>> GetMyFriendsPosts(int userId);
        Task<IEnumerable<RecipePostView>> GetMyFriendsRecipePost(int userId);
        Task<IEnumerable<ProgressPostView>> GetMyFriendsProgressPost(int userId);

    }
}
