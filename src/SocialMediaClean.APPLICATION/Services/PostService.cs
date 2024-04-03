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
        public async Task<int> CreatePostNormalAsync(CreateNormalPostDTO post)
        {
            var Pictures = await uploadFiles.UploadPicturesAsync(post.PicturesDTO);
            var Videos = await uploadFiles.UploadAndCompressVideosAsync(post.VideosDTO);
            post.Videos = Videos;
            post.Pictures = Pictures;
            return await _postRepository.CreatePostNormalAsync(post);
        }
    }
}
