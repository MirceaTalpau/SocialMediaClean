using Dapper;
using SocialMediaClean.DOMAIN.Models.DTOs;
using SocialMediaClean.INFRASTRUCTURE.Interfaces;
using SocialMediaClean.PERSISTANCE.Interfaces;
using System.Data;

namespace SocialMediaClean.PERSISTANCE.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly string CHECK_EXISTING_USER = "usp_CheckExistingUser";
        private readonly string CHANGE_PASSWORD = "usp_ChangePassword";
        private readonly string INSERT_FORGOT_PASSWORD_TOKEN = "usp_InsertForgotPasswordToken";
        private readonly string INSERT_CONFIRM_EMAIL_TOKEN = "usp_InsertEmailConfirmationToken";
        private readonly string GET_CONFIRM_EMAIL_TOKEN = "usp_GetEmailConfirmationToken";
        private readonly string GET_FORGOT_PASSWORD_TOKEN = "uspt_GetForgotPasswordToken";
        private readonly string VALIDATE_EMAIL = "usp_ValidateEmail";
        private readonly string CHECK_EMAIL_VERIFIED = "usp_CheckEmailVerified";
        private readonly string GET_PASSWORD_RESET_TOKEN = "usp_account_GetPasswordResetToken";
        private readonly IDbConnectionFactory _db;

        public AccountRepository(IDbConnectionFactory db)
        {
            _db = db;
        }

        public async Task<string> CheckExistingUserAsync(string email="",string phone="")
        {
            using(var conn = await _db.CreateDbConnectionAsync())
            {
                if(email == string.Empty && phone == string.Empty)
                {
                    throw new ArgumentException("Email or phone must be provided");
                }
                var parameters = new DynamicParameters();
                parameters.Add("Email", email);
                parameters.Add("PhoneNumber", phone);
                string existingUser = await conn.ExecuteScalarAsync<string>(CHECK_EXISTING_USER, parameters, commandType: CommandType.StoredProcedure);
                if(existingUser != null)
                {
                    return existingUser;
                }
                return string.Empty;
            }
        }

        public async Task ChangePasswordAsync(IDPasswordSaltDTO changePassword)
        {
            using (var conn  = await _db.CreateDbConnectionAsync()) 
            {
                var parameters = new DynamicParameters();
                parameters.Add("ID",changePassword.ID);
                parameters.Add("Password",changePassword.Password);
                parameters.Add("PasswordSalt", changePassword.PasswordSalt);
                await conn.ExecuteAsync(CHANGE_PASSWORD,parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<string> GetPasswordResetTokenAsync(int id)
        {
            using (var conn = await _db.CreateDbConnectionAsync())
            {
                var parameters = new DynamicParameters();
                parameters.Add("ID", id);
                string token = await conn.QuerySingleAsync<string>(GET_PASSWORD_RESET_TOKEN, parameters, commandType: CommandType.StoredProcedure);
                return token;
            }
        }

        public async Task InsertForgotPasswordTokenAsync(string token,int ID)
        {
            using (var conn =  await _db.CreateDbConnectionAsync())
            {
                var parameters = new DynamicParameters();
                parameters.Add("PasswordForgotToken", token);
                parameters.Add("ID", ID);
                await conn.ExecuteAsync(INSERT_FORGOT_PASSWORD_TOKEN, parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task UpdateEmailConfirmationTokenAsync(string token,int ID)
        {
            using (var conn = await _db.CreateDbConnectionAsync())
            {
                var parameters = new DynamicParameters();
                parameters.Add("EmailConfirmationToken", token);
                parameters.Add("ID", ID);
                await conn.ExecuteAsync(INSERT_CONFIRM_EMAIL_TOKEN, parameters, commandType: CommandType.StoredProcedure);
            }

        }

        public async Task<string> GetEmailConfirmationTokenAsync(string email)
        {
            using (var conn = await _db.CreateDbConnectionAsync())
            {
                var parameters = new DynamicParameters();
                parameters.Add("Email", email);
                var token = await conn.QuerySingleAsync<string>(GET_CONFIRM_EMAIL_TOKEN, parameters, commandType: CommandType.StoredProcedure);
                return token;
            }
        }

        public async Task<string> GetForgotPasswordToken(string email)
        {
            using (var conn = await _db.CreateDbConnectionAsync())
            {
                var parameters = new DynamicParameters();
                parameters.Add("Email", email);
                var token = await conn.QuerySingleAsync<string>(GET_FORGOT_PASSWORD_TOKEN, parameters, commandType: CommandType.StoredProcedure);
                return token;

            }
        }

        public async Task ValidateEmailAsync(string email)
        {
            using ( var conn = await _db.CreateDbConnectionAsync())
            {
                var parameters = new DynamicParameters();
                parameters.Add("Email", email);
                await conn.ExecuteAsync(VALIDATE_EMAIL, parameters,commandType:CommandType.StoredProcedure);
            }
        }

        public async Task<bool> IsEmailVerifiedAsync(string email)
        {
            using (var conn = await _db.CreateDbConnectionAsync())
            {
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
