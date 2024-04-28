using System.IdentityModel.Tokens.Jwt;

namespace LinkedFit.APPLICATION.Helpers
{
    public class JwtHelper
    {

        public static int GetUserIdFromToken(string token)
        {
            try
            {
                // Remove "Bearer " prefix if present
                token = token.Replace("Bearer ", "");

                // Decode the JWT token
                var tokenHandler = new JwtSecurityTokenHandler();
                var decodedToken = tokenHandler.ReadJwtToken(token);

                // Get the user ID claim from the decoded token
                var userIdClaim = decodedToken.Claims.FirstOrDefault(c => c.Type == "ID");

                // Parse the user ID claim value
                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                {
                    return userId;
                }
            }
            catch (Exception)
            {
                throw;
            }

            return 0;
        }
    }
}
