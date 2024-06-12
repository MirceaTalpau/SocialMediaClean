using Dapper;
using LinkedFit.DOMAIN.Models.DTOs.UserProfile;
using LinkedFit.PERSISTANCE.Interfaces;
using SocialMediaClean.INFRASTRUCTURE.Interfaces;

namespace LinkedFit.PERSISTANCE.Repositories
{
    public class SearchRepository : ISearchRepository
    {
        private readonly IDbConnectionFactory _db;
        public SearchRepository(IDbConnectionFactory db)
        {
            _db = db;
        }

        public async Task<IEnumerable<UserProfileDTO>> SearchUsersAsync(string searchQuery)
        {
            using (var conn = await _db.CreateDbConnectionAsync())
            {
                try
                {
                    //var query = "SELECT * FROM Users WHERE FirstName LIKE @SearchQuery OR LastName LIKE @SearchQuery";
                    var query = "SELECT * FROM Users WHERE FirstName LIKE '%' + @SearchQuery + '%' OR LastName LIKE '%' + @SearchQuery + '%'";
                    var result = await conn.QueryAsync<UserProfileDTO>(query, new { SearchQuery = $"%{searchQuery}%" });
                    return result;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
