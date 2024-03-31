using Dapper;
using LinkedFit.DOMAIN.Models.DTOs.Auth;
using SocialMediaClean.DOMAIN.Models.Entities;
using SocialMediaClean.INFRASTRUCTURE.Interfaces;
using SocialMediaClean.PERSISTANCE.Interfaces;
using System.Data;

namespace SocialMediaClean.PERSISTANCE.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly IDbConnectionFactory _db;
        private readonly string GET_PASSWORD_AND_SALT = "usp_GetPasswordAndSalt";
        private readonly string REGISTER_USER = "usp_RegisterUser";
        private readonly string CHECK_EXISTING_USER = "usp_CheckExistingUser";
        private readonly string CHECK_EMAIL_VERIFIED = "usp_CheckEmailVerified";
        private readonly string CHECK_GMAIL = "usp_CheckGmail";
        public AuthRepository(IDbConnectionFactory db)
        {
            _db = db;
        }

        public async Task<CheckGmailDTO> CheckGmailAsync(string email)
        {
            using( var conn = await _db.CreateDbConnectionAsync())
            {
                var parameters = new DynamicParameters();
                parameters.Add("Email", email);
                parameters.Add("ID", dbType: DbType.Int32, direction: ParameterDirection.Output);
                var result = new CheckGmailDTO();
                await conn.QuerySingleOrDefaultAsync<int>(CHECK_GMAIL, parameters, commandType: CommandType.StoredProcedure);
                result.ID = parameters.Get<int>("ID");
                if(result.ID != 0)
                {
                    result.Exists = true;
                    return result;
                }
                return result;
            }
        }

        public async Task<bool> RegisterUserAsync(RegisterRequestDTO user,string confirmationToken = "")
        {
            using ( var conn =await _db.CreateDbConnectionAsync())
            {
                
                var parameters = new DynamicParameters();
                
                parameters.Add("Email", user.Email);
                if (user.BirthDay == null && user.PasswordSalt == null && user.Password == null && user.PhoneNumber == null && user.Gender == null && confirmationToken == "")
                {
                    parameters.Add("FirstName", user.FirstName);
                    parameters.Add("LastName", user.LastName);
                    parameters.Add("ProfileAvatar", user.ProfileAvatar);
                    await conn.QueryAsync(REGISTER_USER, parameters, commandType: CommandType.StoredProcedure);
                    return true;
                }
                parameters.Add("PhoneNumber", user.PhoneNumber);
                var existingUser = await conn.QuerySingleOrDefaultAsync<string>(CHECK_EXISTING_USER, parameters, commandType: CommandType.StoredProcedure);
                if (existingUser != null)
                {
                    return false;
                }
                
                parameters.Add("FirstName", user.FirstName);
                parameters.Add("LastName", user.LastName);
                parameters.Add("Password", user.Password);
                parameters.Add("PasswordSalt", user.PasswordSalt);
                parameters.Add("BirthDay", user.BirthDay);
                parameters.Add("Gender", user.Gender);
                parameters.Add("EmailVerifyToken", confirmationToken);
                await conn.QueryAsync(REGISTER_USER, parameters, commandType: CommandType.StoredProcedure);
                return true;
            }
        }

        public async Task<PasswordAndSalt> GetPasswordAndSaltAsync(string email)
        {
            using ( var conn = await _db.CreateDbConnectionAsync())
            {
                var parameters = new DynamicParameters();
                parameters.Add("Email", email);
                PasswordAndSalt passwordAndSalt = await conn.QueryFirstAsync<PasswordAndSalt>(GET_PASSWORD_AND_SALT, parameters,commandType: CommandType.StoredProcedure);
                return passwordAndSalt;
            }
        }

        public async Task<bool> IsEmailVerifiedAsync(string email)
        {
            using (var conn = await _db.CreateDbConnectionAsync())
            {
                //MODIFICA AICI
                var parameters = new DynamicParameters();
                parameters.Add("Email", email);
                parameters.Add("Verified", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                await conn.QueryAsync(CHECK_EMAIL_VERIFIED, parameters, commandType: CommandType.StoredProcedure);
                var result = parameters.Get<bool>("Verified");
                return result;
            }
        }
        
    }
}
