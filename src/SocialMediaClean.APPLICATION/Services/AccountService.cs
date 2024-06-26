﻿using LinkedFit.DOMAIN.Models.DTOs;
using Microsoft.IdentityModel.Tokens;
using SocialMediaClean.APPLICATION.Contracts;
using SocialMediaClean.APPLICATION.Response;
using SocialMediaClean.DOMAIN.Models.DTOs;
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

        //private string GenerateResetPasswordToken(int userID)
        //{
        //    byte[] userIdBytes = BitConverter.GetBytes(userID);
        //    byte[] time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
        //    byte[] key = Guid.NewGuid().ToByteArray();
        //    byte[] tokenBytes = userIdBytes.Concat(time).Concat(key).ToArray();
        //    string token = Convert.ToBase64String(tokenBytes);
        //    return token;
        //}

        //public bool VerifyResetPasswordToken(string token,out int userID)
        //{
        //    byte[] tokenBytes = Convert.FromBase64String(token);
        //    int userIdSize = sizeof(int);
        //    byte[] userIdBytes = tokenBytes.Take(userIdSize).ToArray();
        //    userID = BitConverter.ToInt32(userIdBytes, 0);
        //    byte[] timestampBytes = tokenBytes.Skip(userIdSize).ToArray();

        //    // Convert timestamp bytes back to DateTime
        //    DateTime timestamp = DateTime.FromBinary(BitConverter.ToInt64(timestampBytes, 0));

        //    if (DateTime.UtcNow - timestamp > TimeSpan.FromMinutes(5))
        //    {
        //        return false;
        //    }
        //    return true;
        //}

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
            await SendEmailAsync(email, "Password reset", $"Click <a href=\"http://localhost:4200/auth/confirm/password/{token}\">here</a> to reset your password.");            
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
            await SendEmailAsync(email, "Email confirmation", $"Click <a href=\"http://localhost:4200/auth/confirm/email/{email}/{confirmationToken}\">here</a> to confirm your email.");
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
