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
                var posts = await _postRepository.GetAllNormalPostsAsync();
                posts.OrderByDescending(post => post.CreatedAt).ToList();
                foreach (NormalPostView post in posts)
                {
                    post.Media = await _postRepository.GetMediaPostAsync(post.ID);
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
                IEnumerable<RecipePostView> posts = await _postRepository.GetAllRecipePostsAsync();
                foreach (RecipePostView post in posts)
                {
                    post.Media = await _postRepository.GetMediaPostAsync(post.PostID);
                    post.Ingredients = await _postRepository.GetIngredientsAsync(post.RecipeID);
                    post.Media.ToList().Sort(new MediaPostComparer());
                }
                return posts;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<ProgressPostView>> GetAllPublicProgressPosts()
        {
            try
            {
                IEnumerable<ProgressPostView> posts = await _postRepository.GetPublicProgressPostsAsync();
                foreach (ProgressPostView post in posts)
                {
                    List<MediaPostView> medias = new List<MediaPostView>();
                    var before = new MediaPostView
                    {
                        PostID = post.PostID,
                        PictureCreatedAt = post.BeforeDate,
                        PictureURI = post.BeforePictureURI,
                        Caption = post.BeforeWeight.ToString()
                    };
                    var after = new MediaPostView
                    {
                        PostID = post.PostID,
                        PictureCreatedAt = post.AfterDate,
                        PictureURI = post.AfterPictureURI,
                        Caption = post.AfterWeight.ToString()
                    };
                    medias.Add(before);
                    medias.Add(after);
                    post.Media = medias;
                }
                return posts;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
