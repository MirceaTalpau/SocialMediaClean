using Dapper;
using SocialMediaClean.APPLICATION.DTOs;
using SocialMediaClean.DOMAIN.Models.DTOs;
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
        public AuthRepository(IDbConnectionFactory db)
        {
            _db = db;
        }

        public async Task<bool> RegisterUserAsync(RegisterRequestDTO user)
        {
            using ( var conn =await _db.CreateDbConnectionAsync())
            {
                
                var parameters = new DynamicParameters();
                
                parameters.Add("Email", user.Email);
                parameters.Add("PhoneNumber", user.PhoneNumber);
                string existingUser = await conn.ExecuteScalarAsync<string>(CHECK_EXISTING_USER, parameters, commandType: CommandType.StoredProcedure);
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
        
    }
}
