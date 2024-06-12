using LinkedFit.DOMAIN.Models.DTOs.UserProfile;
using LinkedFit.DOMAIN.Models.Views;

namespace LinkedFit.PERSISTANCE.Interfaces
{
    public interface IUserProfileRepository
    {
        Task<UserProfileDTO> GetUserProfileAsync(int userID);
        Task<IEnumerable<NormalPostView>> GetUserProfileNormalPosts(int currentUserID, int profileUserID);
        Task UpdateUserProfileAsync(UserProfileDTO userProfile);
    }
}