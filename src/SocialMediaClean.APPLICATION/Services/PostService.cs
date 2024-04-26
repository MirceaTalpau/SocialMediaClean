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
        private async Task DeleteUploadedFiles(CreateNormalPostDTO post)
        {
            
           uploadFiles.DeleteUploadedFiles(post);
        }
        public async Task<int> CreatePostNormalAsync(CreateNormalPostDTO post)
        {
            try
            {
                await UploadFiles(post);
                var postId = await _postRepository.CreatePostNormalAsync(post);
                return postId;
            }
            catch (Exception)
            {
                await DeleteUploadedFiles(post);
                throw;
            }

        }
        public async Task<int> CreatePostRecipeAsync(CreateRecipePostDTO post)
        {
            try
            {
                await UploadFiles(post);
                var postId = await _postRepository.CreatePostRecipeAsync(post);
                return postId;
            }
            catch (Exception)
            {
                await DeleteUploadedFiles(post);
                throw;
            }

        }

        public async Task<int> CreatePostProgressAsync(CreateProgressPostDTO post)
        {
            try
            {
                var beforePicture = await uploadFiles.UploadPictureAsync(post.BeforePicture);
                post.BeforePictureUri = beforePicture;
                var afterPicture = await uploadFiles.UploadPictureAsync(post.AfterPicture);
                post.AfterPictureUri = afterPicture;
                var postId = await _postRepository.CreatePostProgressAsync(post);

                if(postId == 0)
                {
                    uploadFiles.DeleteUploadedFiles(post);
                }

                return postId;
            }
            catch (Exception)
            {
                await DeleteUploadedFiles(post);
                throw;
            }

        }

        public async Task<bool> DeletePostAsync(int postId)
        {
            try
            {
                var medias = await _postRepository.DeletePostAsync(postId);
                uploadFiles.DeleteUploadedFiles(medias);
                return true;
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
