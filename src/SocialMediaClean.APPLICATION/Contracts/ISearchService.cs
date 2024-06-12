using LinkedFit.DOMAIN.Models.DTOs.UserProfile;

namespace LinkedFit.APPLICATION.Contracts
{
    public interface ISearchService
    {
        Task<IEnumerable<UserProfileDTO>> Search(string query);
    }
}