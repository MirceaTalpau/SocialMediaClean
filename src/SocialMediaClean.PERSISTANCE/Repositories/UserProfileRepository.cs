using Dapper;
using LinkedFit.DOMAIN.Models.DTOs.UserProfile;
using LinkedFit.DOMAIN.Models.Views;
using LinkedFit.PERSISTANCE.Interfaces;
using SocialMediaClean.INFRASTRUCTURE.Interfaces;
using System.Data;

namespace LinkedFit.PERSISTANCE.Repositories
{
    public class UserProfileRepository : IUserProfileRepository
    {
        private readonly IDbConnectionFactory _db;
        public UserProfileRepository(IDbConnectionFactory db)
        {
            _db = db;
        }

        public async Task<UserProfileDTO> GetUserProfileAsync(int userID)
        {
            using (var conn = await _db.CreateDbConnectionAsync())
            {
                try
                {
                    var query = "SELECT * FROM Users WHERE ID = @ID AND EmailVerified = 1";
                    var result = await conn.QueryFirstOrDefaultAsync<UserProfileDTO>(query, new { ID = userID });
                    return result;
                }
                catch (Exception)
                {
                    throw;
                }
                
            }
        }
        public async Task UpdateUserProfileAsync(UserProfileDTO userProfile)
        {
            using (var conn = await _db.CreateDbConnectionAsync())
            {
                try
                {
                    var query = "UPDATE Users SET FirstName = @FirstName, LastName = @LastName, Email = @Email, PhoneNumber = @PhoneNumber, ProfilePictureURL = @ProfilePictureURL, Bio = @Bio, City = @City, Country = @Country, Address = @Address WHERE ID = @ID";
                    await conn.ExecuteAsync(query, userProfile);
                }
                catch (Exception)
                {
                    throw;
                }
                
            }
        }

        public async Task<IEnumerable<NormalPostView>> GetUserProfileNormalPosts(int currentUserID, int profileUserID)
        {
            using (var conn = await _db.CreateDbConnectionAsync())
            {
                try
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@CurrentUserID", currentUserID);
                    parameters.Add("@ProfileUserID", profileUserID);
                    var posts = await conn.QueryAsync<NormalPostView>("usp_Post_GetUserProfilePosts", parameters, commandType: CommandType.StoredProcedure);
                    return posts;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
