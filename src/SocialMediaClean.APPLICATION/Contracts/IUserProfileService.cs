using LinkedFit.DOMAIN.Models.DTOs.UserProfile;
using LinkedFit.DOMAIN.Models.Views;

namespace LinkedFit.APPLICATION.Contracts
{
    public interface IUserProfileService
    {
        Task<IEnumerable<NormalPostView>> GetNormalPosts(int currentUserID, int profileUserID);
        Task<UserProfileDTO> GetUserProfile(int currentUserID, int userProfileID);
        Task UpdateUserProfile(UserProfileDTO userProfile);
    }
}