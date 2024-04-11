using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace LinkedFit.APPLICATION.Validators
{
    public class JwtValidator
    {
        private readonly string _jwtSecret;
        private readonly IConfiguration _configuration;

        public JwtValidator(IConfiguration configuration)
        {
            _configuration = configuration;
            _jwtSecret = _configuration.GetSection("Jwt")["Key"];
        }
        public bool IsJwtValid(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret)), // Convert string key to byte array
                ValidateIssuer = false, 
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero 
            };

            try
            {
                SecurityToken validatedToken;
                tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
                // If the token hasn't thrown an exception, it's valid
                return true;
            }
            catch (SecurityTokenException)
            {
                // Token is not valid
                return false;
            }
        }

        public bool IsJwtExpired(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

            if (jwtToken == null)
                return true;

            var expirationTime = jwtToken.ValidTo;
            return expirationTime <= DateTime.UtcNow;
        }

    }
}
