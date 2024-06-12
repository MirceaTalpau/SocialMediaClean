using LinkedFit.APPLICATION.Contracts;
using LinkedFit.DOMAIN.Models.DTOs.UserProfile;
using LinkedFit.PERSISTANCE.Interfaces;

namespace LinkedFit.APPLICATION.Services
{
    public class SearchService : ISearchService
    {
        private readonly ISearchRepository _searchRepository;
        public SearchService(ISearchRepository searchRepository)
        {
            _searchRepository = searchRepository;
        }

        public async Task<IEnumerable<UserProfileDTO>> Search(string query)
        {
            return await _searchRepository.SearchUsersAsync(query);
        }
    }
}
