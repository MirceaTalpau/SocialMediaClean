using LinkedFit.APPLICATION.Contracts;
using LinkedFit.DOMAIN.Models.DTOs.Posts;
using LinkedFit.INFRASTRUCTURE.Implementation;
using LinkedFit.PERSISTANCE.Interfaces;

namespace LinkedFit.APPLICATION.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly UploadFiles uploadFiles = new UploadFiles();
        public PostService(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }
        public async Task<int> CreatePostNormalAsync(CreateNormalPostDTO post)
        {
            var Pictures = await uploadFiles.UploadPicturesAsync(post.PicturesDTO);
            post.Pictures = Pictures;
            return await _postRepository.CreatePostNormalAsync(post);
        }
    }
}
