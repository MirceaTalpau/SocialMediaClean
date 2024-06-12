using LinkedFit.DOMAIN.Models.DTOs.UserProfile;

namespace LinkedFit.PERSISTANCE.Interfaces
{
    public interface ISearchRepository
    {
        Task<IEnumerable<UserProfileDTO>> SearchUsersAsync(string searchQuery);
    }
}