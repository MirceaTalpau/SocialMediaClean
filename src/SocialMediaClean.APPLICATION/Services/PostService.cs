using LinkedFit.APPLICATION.Contracts;
using LinkedFit.DOMAIN.Models.DTOs.Posts;
using LinkedFit.INFRASTRUCTURE.Implementation;
using LinkedFit.PERSISTANCE.Interfaces;

namespace LinkedFit.APPLICATION.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly UploadFilesService uploadFiles = new UploadFilesService();
        public PostService(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        private async Task UploadFiles(CreateNormalPostDTO post)
        {
            if (post.PicturesDTO != null)
            {
                var Pictures = await uploadFiles.UploadPicturesAsync(post.PicturesDTO);
                post.Pictures = Pictures;
            }
            if (post.VideosDTO != null)
            {
                var Videos = await uploadFiles.UploadAndCompressVideosAsync(post.VideosDTO);
                post.Videos = Videos;
            }
        }
        public async Task<int> CreatePostNormalAsync(CreateNormalPostDTO post)
        {
            await UploadFiles(post);
            return await _postRepository.CreatePostNormalAsync(post);
        }
        public async Task<int> CreatePostRecipeAsync(CreateRecipePostDTO post)
        {
            await UploadFiles(post);
            return await _postRepository.CreatePostRecipeAsync(post);
        }
    }
}
