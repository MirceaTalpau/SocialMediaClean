using LinkedFit.DOMAIN.Models.DTOs.Auth;
using Microsoft.IdentityModel.Tokens;
using SocialMediaClean.APPLICATION.Contracts;
using SocialMediaClean.APPLICATION.Response;
using SocialMediaClean.INFRASTRUCTURE.Interfaces;
using SocialMediaClean.INFRASTRUCTURE.Models;
using SocialMediaClean.PERSISTANCE.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SocialMediaClean.APPLICATION.Services
{
    public class AccountService : IAccountService
    {
        private IConfiguration _config;
        private readonly IAccountRepository _accountRepository;
        private readonly IMailService _mailService;
        private static readonly Byte[] _privateKey = new Byte[] {0x2B, 0xF4, 0x55, 0xAD, 0x4C, 0x54, 0x75, 0x7F, 0xB7, 0x92, 0x69, 0xFD, 0x1A, 0xCB, 0x2F, 0x56, 0x11, 0x28, 0x69, 0xE6, 0x79, 0x75, 0xB7, 0xCA, 0x4C, 0x4F, 0xD1, 0x10, 0xDC, 0xA4, 0x6E, 0x06, }; // NOTE: You should use a private-key that's a LOT longer than just 4 bytes.
        private static readonly TimeSpan _passwordResetExpiry = TimeSpan.FromMinutes(5);
        private const Byte _version = 1; // Increment this whenever the structure of the message changes.
        const int keySize = 64;
        const int iterations = 350000;
        private readonly string key;
        HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;
        public AccountService(IAccountRepository accountRepository,IMailService mailService, IConfiguration config)
        {
            _accountRepository = accountRepository;
            _mailService = mailService;
            _config = config;
            key = _config["Jwt:Key"];
        }

        private async Task SendEmailAsync(string receiver, string subject, string body)
        {
            var mail = new MailRequest
            {
                Receiver = receiver,
                Subject = subject,
                Body = body
            };
            await _mailService.SendEmailAsync(mail);
        }

        public string GenerateResetPasswordToken(int userID)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userID.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            };
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(5),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha256)
                               );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }



        public async Task<ResetTokenUserIdDTO> VerifyResetPasswordTokenAsync(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.key));
            var result = new ResetTokenUserIdDTO();
            var validationParameters = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateLifetime = true,
                ValidateAudience = false,
                ValidateIssuer = false,
                ClockSkew = TimeSpan.Zero
            };
            try
            {
                var securityToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
                var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
                var userIdClaim = securityToken.Claims.FirstOrDefault(x => x.Type == "sub");
                var tokenInDb = await _accountRepository.GetPasswordResetTokenAsync(Int32.Parse(securityToken.Subject));
                if (tokenInDb == token)
                {
                    result.UserId = Int32.Parse(userIdClaim.Value);
                    result.IsValid = true;
                    return result;
                }
            }
            catch
            {
                return result;
            }
            return result;

        }

        private async Task<String> GenerateBodyForEmail(string email, string token,string type)
        {
            StringBuilder body = new StringBuilder();
            if (type == "reset")
            {
                body = new StringBuilder();
                body.Append("<!DOCTYPE html>");
                body.Append("<html lang=\"en\">");
                body.Append("<head>");
                body.Append("<meta charset=\"UTF-8\">");
                body.Append("<meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\">");
                body.Append("<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">");
                body.Append("<title>Reset Password</title>");
                body.Append("<meta name=\"description\" content=\"Reset Password.\">");
                body.Append("<style type=\"text/css\">");
                body.Append("a:hover {text-decoration: underline !important;}");
                body.Append("</style>");
                body.Append("</head>");
                body.Append("<body marginheight=\"0\" topmargin=\"0\" marginwidth=\"0\" style=\"margin: 0px; background-color: #f2f3f8;\" leftmargin=\"0\">");
                body.Append("<table cellspacing=\"0\" border=\"0\" cellpadding=\"0\" width=\"100%\" bgcolor=\"#f2f3f8\" style=\"@import url(https://fonts.googleapis.com/css?family=Rubik:300,400,500,700|Open+Sans:300,400,600,700); font-family: 'Open Sans', sans-serif;\">");
                body.Append("<tr>");
                body.Append("<td>");
                body.Append("<table style=\"background-color: #f2f3f8; max-width:670px;  margin:0 auto;\" width=\"100%\" border=\"0\" align=\"center\" cellpadding=\"0\" cellspacing=\"0\">");
                body.Append("<tr>");
                body.Append("<td style=\"height:80px;\">&nbsp;</td>");
                body.Append("</tr>");
                body.Append("<tr>");
                body.Append("<td style=\"text-align:center;\">");
                body.Append("<a href=\"https://rakeshmandal.com\" title=\"logo\" target=\"_blank\">");
                body.Append("<img width=\"60\" src=\"https://i.ibb.co/hL4XZp2/android-chrome-192x192.png\" title=\"logo\" alt=\"logo\">");
                body.Append("</a>");
                body.Append("</td>");
                body.Append("</tr>");
                body.Append("<tr>");
                body.Append("<td style=\"height:20px;\">&nbsp;</td>");
                body.Append("</tr>");
                body.Append("<tr>");
                body.Append("<td>");
                body.Append("<table width=\"95%\" border=\"0\" align=\"center\" cellpadding=\"0\" cellspacing=\"0\" style=\"max-width:670px;background:#fff; border-radius:3px; text-align:center;-webkit-box-shadow:0 6px 18px 0 rgba(0,0,0,.06);-moz-box-shadow:0 6px 18px 0 rgba(0,0,0,.06);box-shadow:0 6px 18px 0 rgba(0,0,0,.06);\">");
                body.Append("<tr>");
                body.Append("<td style=\"height:40px;\">&nbsp;</td>");
                body.Append("</tr>");
                body.Append("<tr>");
                body.Append("<td style=\"padding:0 35px;\">");
                body.Append("<h1 style=\"color:#1e1e2d; font-weight:500; margin:0;font-size:32px;font-family:'Rubik',sans-serif;\">You have requested to reset your password</h1>");
                body.Append("<span style=\"display:inline-block; vertical-align:middle; margin:29px 0 26px; border-bottom:1px solid #cecece; width:100px;\"></span>");
                body.Append("<p style=\"color:#455056; font-size:15px;line-height:24px; margin:0;\">");
                body.Append("We cannot simply send you your old password. A unique link to reset your password has been generated for you. To reset your password, click the following link and follow the instructions.");
                body.Append("</p>");
                body.Append($"<a href=\"http://localhost:4200/auth/confirm/password/{token}\" style=\"background:#20e277;text-decoration:none !important; font-weight:500; margin-top:35px; color:#fff;text-transform:uppercase; font-size:14px;padding:10px 24px;display:inline-block;border-radius:50px;\">Reset Password</a>");
                body.Append("</td>");
                body.Append("</tr>");
                body.Append("<tr>");
                body.Append("<td style=\"height:40px;\">&nbsp;</td>");
                body.Append("</tr>");
                body.Append("</table>");
                body.Append("</td>");
                body.Append("</tr>");
                body.Append("<tr>");
                body.Append("<td style=\"height:20px;\">&nbsp;</td>");
                body.Append("</tr>");
                body.Append("<tr>");
                body.Append("<td style=\"text-align:center;\">");
                body.Append("<p style=\"font-size:14px; color:rgba(69, 80, 86, 0.7411764705882353); line-height:18px; margin:0 0 0;\">&copy; <strong>www.rakeshmandal.com</strong></p>");
                body.Append("</td>");
                body.Append("</tr>");
                body.Append("<tr>");
                body.Append("<td style=\"height:80px;\">&nbsp;</td>");
                body.Append("</tr>");
                body.Append("</table>");
                body.Append("</td>");
                body.Append("</tr>");
                body.Append("</table>");
                body.Append("</body>");
                body.Append("</html>");
            }
            if(type == "confirm")
            {
                body = new StringBuilder();
                body.Append("<!DOCTYPE html>");
                body.Append("<html lang=\"en\">");
                body.Append("<head>");
                body.Append("<meta charset=\"UTF-8\">");
                body.Append("<meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\">");
                body.Append("<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">");
                body.Append("<title>Confirm Email</title>");
                body.Append("<meta name=\"description\" content=\"Confirm Email.\">");
                body.Append("<style type=\"text/css\">");
                body.Append("a:hover {text-decoration: underline !important;}");
                body.Append("</style>");
                body.Append("</head>");
                body.Append("<body marginheight=\"0\" topmargin=\"0\" marginwidth=\"0\" style=\"margin: 0px; background-color: #f2f3f8;\" leftmargin=\"0\">");
                body.Append("<table cellspacing=\"0\" border=\"0\" cellpadding=\"0\" width=\"100%\" bgcolor=\"#f2f3f8\" style=\"@import url(https://fonts.googleapis.com/css?family=Rubik:300,400,500,700|Open+Sans:300,400,600,700); font-family: 'Open Sans', sans-serif;\">");
                body.Append("<tr>");
                body.Append("<td>");
                body.Append("<table style=\"background-color: #f2f3f8; max-width:670px;  margin:0 auto;\" width=\"100%\" border=\"0\" align=\"center\" cellpadding=\"0\" cellspacing=\"0\">");
                body.Append("<tr>");
                body.Append("<td style=\"height:80px;\">&nbsp;</td>");
                body.Append("</tr>");
                body.Append("<tr>");
                body.Append("<td style=\"text-align:center;\">");
                body.Append("<a href=\"https://rakeshmandal.com\" title=\"logo\" target=\"_blank\">");
                body.Append("<img width=\"60\" src=\"https://i.ibb.co/hL4XZp2/android-chrome-192x192.png\" title=\"logo\" alt=\"logo\">");
                body.Append("</a>");
                body.Append("</td>");
                body.Append("</tr>");
                body.Append("<tr>");
                body.Append("<td style=\"height:20px;\">&nbsp;</td>");
                body.Append("</tr>");
                body.Append("<tr>");
                body.Append("<td>");
                body.Append("<table width=\"95%\" border=\"0\" align=\"center\" cellpadding=\"0\" cellspacing=\"0\" style=\"max-width:670px;background:#fff; border-radius:3px; text-align:center;-webkit-box-shadow:0 6px 18px 0 rgba(0,0,0,.06);-moz-box-shadow:0 6px 18px 0 rgba(0,0,0,.06);box-shadow:0 6px 18px 0 rgba(0,0,0,.06);\">");
                body.Append("<tr>");
                body.Append("<td style=\"height:40px;\">&nbsp;</td>");
                body.Append("</tr>");
                body.Append("<tr>");
                body.Append("<td style=\"padding:0 35px;\">");
                body.Append("<h1 style=\"color:#1e1e2d; font-weight:500; margin:0;font-size:32px;font-family:'Rubik',sans-serif;\">You need to confirm your email.</h1>");
                body.Append("<span style=\"display:inline-block; vertical-align:middle; margin:29px 0 26px; border-bottom:1px solid #cecece; width:100px;\"></span>");
                body.Append("<p style=\"color:#455056; font-size:15px;line-height:24px; margin:0;\">");
                body.Append("We sent you a unique link for you to confirm your email.");
                body.Append("</p>");
                body.Append($"<a href=\"http://localhost:4200/auth/confirm/email/{email}/{token}\" style=\"background:#20e277;text-decoration:none !important; font-weight:500; margin-top:35px; color:#fff;text-transform:uppercase; font-size:14px;padding:10px 24px;display:inline-block;border-radius:50px;\">Confirm email</a>");
                body.Append("</td>");
                body.Append("</tr>");
                body.Append("<tr>");
                body.Append("<td style=\"height:40px;\">&nbsp;</td>");
                body.Append("</tr>");
                body.Append("</table>");
                body.Append("</td>");
                body.Append("</tr>");
                body.Append("<tr>");
                body.Append("<td style=\"height:20px;\">&nbsp;</td>");
                body.Append("</tr>");
                body.Append("<tr>");
                body.Append("<td style=\"text-align:center;\">");
                body.Append("<p style=\"font-size:14px; color:rgba(69, 80, 86, 0.7411764705882353); line-height:18px; margin:0 0 0;\">&copy; <strong>www.rakeshmandal.com</strong></p>");
                body.Append("</td>");
                body.Append("</tr>");
                body.Append("<tr>");
                body.Append("<td style=\"height:80px;\">&nbsp;</td>");
                body.Append("</tr>");
                body.Append("</table>");
                body.Append("</td>");
                body.Append("</tr>");
                body.Append("</table>");
                body.Append("</body>");
                body.Append("</html>");
            }
            return body.ToString();
            
        }

        public async Task<BaseResponse> SendPasswordResetMailAsync(string email)
        {
            var response = new BaseResponse();
            var verified = await _accountRepository.IsEmailVerifiedAsync(email);
            if (!verified) 
            {
                response.Message = "Email is not verified!";
                return response; 
            }
            var userIDString = await _accountRepository.CheckExistingUserAsync(email,null);
            var user = int.Parse(userIDString);
            var token = GenerateResetPasswordToken(user);
            var body = await GenerateBodyForEmail(email, token, "reset");
            await SendEmailAsync(email, "Password reset", body.ToString());
            //await SendEmailAsync(email, "Password reset", $"Click <a href=\"http://localhost:4200/auth/confirm/password/{token}\">here</a> to reset your password.");            
            await _accountRepository.InsertForgotPasswordTokenAsync(token, user);
            response.Success = true;
            response.Code = 200;
            response.Message ="OK";
            return response;
        }

        public async Task<BaseResponse> ResendEmailConfirmationTokenAsync(string email)
        {
            var response = new BaseResponse();
            var userIDString = await _accountRepository.CheckExistingUserAsync(email,null);
            if (userIDString.IsNullOrEmpty()) { return response; }
            int userID = int.Parse(userIDString);
            bool verified = await _accountRepository.IsEmailVerifiedAsync(email);
            if (verified)
            {
                response.Message = "User is already verified!";
                return response;
            }
            var confirmationToken = Guid.NewGuid().ToString();
            await _accountRepository.UpdateEmailConfirmationTokenAsync(confirmationToken, userID);
            var body = await GenerateBodyForEmail(email, confirmationToken, "confirm");
            await SendEmailAsync(email, "Email confirmation", body.ToString());
            response.Success = true;
            response.Code = 200;
            response.Message = "Succesfully sent confirmation email!";
            return response;
        }

        public async Task<BaseResponse> VerifyEmailAsync(string email,string token)
        {
            var response = new BaseResponse();
            var DbToken = await _accountRepository.GetEmailConfirmationTokenAsync(email);
            if (DbToken == token)
            {
                await _accountRepository.ValidateEmailAsync(email);
                response.Success = true;
                response.Code = 200;
                response.Message = "Succesfully verified email!";
                return response;
            }
            response.Message = "There was an error verifying the email!";
            return response;
        }


        private String CreatePasswordResetHmacCode(Int32 userId)
        {
            Byte[] message = Enumerable.Empty<Byte>()
                .Append(_version)
                .Concat(BitConverter.GetBytes(userId))
                .Concat(BitConverter.GetBytes(DateTime.UtcNow.ToBinary()))
                .ToArray();

            using (HMACSHA256 hmacSha256 = new HMACSHA256(key: _privateKey))
            {
                Byte[] hash = hmacSha256.ComputeHash(buffer: message, offset: 0, count: message.Length);

                Byte[] outputMessage = message.Concat(hash).ToArray();
                String outputCodeB64 = Convert.ToBase64String(outputMessage);
                String outputCode = outputCodeB64.Replace('+', '-').Replace('/', '_');
                return outputCode;
            }
        }

        public bool VerifyPasswordResetHmacCode(String codeBase64Url, out Int32 userId)
        {
            userId = 0;
            string base64 = codeBase64Url.Replace('-', '+').Replace('_', '/');
            byte[] message = Convert.FromBase64String(base64);

            byte version = message[0];
            if (version < _version) return false;

            userId = BitConverter.ToInt32(message, startIndex: 1); // Reads bytes message[1,2,3,4]
            long createdUtcBinary = BitConverter.ToInt64(message, startIndex: 1 + sizeof(int)); // Reads bytes message[5,6,7,8,9,10,11,12]

            DateTime createdUtc = DateTime.FromBinary(createdUtcBinary);
            if (createdUtc.Add(_passwordResetExpiry) < DateTime.UtcNow) return false;

            const int _messageLength = 1 + sizeof(int) + sizeof(long); // 1 + 4 + 8 == 13

            using (HMACSHA256 hmacSha256 = new HMACSHA256(key: _privateKey))
            {
                byte[] hash = hmacSha256.ComputeHash(message, offset: 0, count: _messageLength);

                byte[] messageHash = message.Skip(_messageLength).ToArray();
                return Enumerable.SequenceEqual(hash, messageHash);
            }
        }
        public async Task ChangePassword(ChangePasswordDTO passwordDTO, Int32 userID)
        {
            if(passwordDTO.Password != passwordDTO.ConfirmPassword) return;
            var idPasswordSalt = new IDPasswordSaltDTO();
            var salt = RandomNumberGenerator.GetBytes(keySize);
            idPasswordSalt.Password = HashPasword(passwordDTO.Password,salt);
            idPasswordSalt.PasswordSalt = Convert.ToHexString(salt);
            idPasswordSalt.ID = userID;
            await _accountRepository.ChangePasswordAsync(idPasswordSalt);

        }

        private string HashPasword(string password, byte[] salt)
        {
            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                salt,
                iterations,
                hashAlgorithm,
                keySize);
            return Convert.ToHexString(hash);
        }
        private byte[] ConvertSaltToByte(string passwordSalt)
        {
            byte[] salt = new byte[passwordSalt.Length / 2];

            for (int i = 0; i < salt.Length; i++)
            {
                salt[i] = Convert.ToByte(passwordSalt.Substring(i * 2, 2), 16);
            }
            return salt;
        }
    }
}
