using LinkedFit.APPLICATION.Comparer;
using LinkedFit.APPLICATION.Contracts;
using LinkedFit.DOMAIN.Models.DTOs.UserProfile;
using LinkedFit.DOMAIN.Models.Views;
using LinkedFit.INFRASTRUCTURE.Interfaces;
using LinkedFit.PERSISTANCE.Interfaces;

namespace LinkedFit.APPLICATION.Services
{
    public class UserProfileService : IUserProfileService
    {
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IPostRepository _postRepository;
        private readonly IUploadFilesService _uploadFilesService;

        public UserProfileService(IUserProfileRepository userProfileRepository, IPostRepository postRepository, IUploadFilesService uploadFilesService)
        {
            _userProfileRepository = userProfileRepository;
            _postRepository = postRepository;
            _uploadFilesService = uploadFilesService;
        }

        public async Task<UserProfileDTO> GetUserProfile(int currentUserID, int userProfileID)
        {
            try
            {
                var userProfile = await _userProfileRepository.GetUserProfileAsync(userProfileID);
                userProfile.Posts = await GetNormalPosts(currentUserID, userProfileID);

                return userProfile;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateUserProfile(UserProfileDTO userProfile)
        {
            try
            {
                if(userProfile.ProfilePicture == null)
                {
                    await _userProfileRepository.UpdateUserProfileAsync(userProfile);
                }
                else { 
                var newUrl = await _uploadFilesService.UploadPictureAsync(userProfile.ProfilePicture);
                userProfile.ProfilePictureURL = newUrl;
                await _userProfileRepository.UpdateUserProfileAsync(userProfile);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<NormalPostView>> GetNormalPosts(int currentUserID, int profileUserID)
        {
            try
            {
                var posts = await _userProfileRepository.GetUserProfileNormalPosts(currentUserID, profileUserID);
                posts = posts.OrderByDescending(post => post.CreatedAt)
                    .ThenByDescending(post => post.SharedAt)
                    .ToList();
                foreach (NormalPostView post in posts)
                {
                    post.Media = await _postRepository.GetMediaPostAsync(post.PostID);
                    post.Media.ToList().Sort(new MediaPostComparer());
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
