using LinkedFit.APPLICATION.Comparer;
using LinkedFit.APPLICATION.Contracts;
using LinkedFit.DOMAIN.Models.Entities.Posts;
using LinkedFit.DOMAIN.Models.Views;
using LinkedFit.PERSISTANCE.Interfaces;

namespace LinkedFit.APPLICATION.Services
{
    public class FeedService : IFeedService
    {
        private readonly IPostRepository _postRepository;

        public FeedService(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }
        public async Task<IEnumerable<NormalPostView>> GetAllPublicNormalPosts()
        {
            try
            {
                var posts = await _postRepository.GetAllNormalPosts();
                posts.OrderByDescending(post => post.CreatedAt).ToList();
                foreach (NormalPostView post in posts)
                {
                    post.Media = await _postRepository.GetMediaPost(post.ID);
                    post.Media.ToList().Sort(new MediaPostComparer());
                }
                return posts;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IEnumerable<RecipePostView>> GetAllRecipePosts()
        {
            try
            {
                IEnumerable<RecipePostView> posts = await _postRepository.GetAllRecipePosts();
                return posts;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
