using AutoMapper;
using LinkedFit.APPLICATION.Validators;
using LinkedFit.DOMAIN.Models.DTOs.Auth;
using Microsoft.IdentityModel.Tokens;
using SocialMediaClean.APPLICATION.Contracts;
using SocialMediaClean.APPLICATION.Requests;
using SocialMediaClean.APPLICATION.Response;
using SocialMediaClean.APPLICATION.Validators;
using SocialMediaClean.INFRASTRUCTURE.Interfaces;
using SocialMediaClean.INFRASTRUCTURE.Models;
using SocialMediaClean.PERSISTANCE.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SocialMediaClean.APPLICATION.Services
{
    public class AuthService : IAuthService
    {
        private readonly RegisterRequestValidator _registerValidator;
        private readonly LoginRequestValidator _loginValidator;
        private readonly IAuthRepository _authRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly IMailService _mailService;
        const int keySize = 64;
        const int iterations = 350000;
        HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;

        public AuthService(IAuthRepository authRepository,IMapper mapper, IConfiguration config,IMailService mailService)
        {
            _registerValidator = new RegisterRequestValidator();
            _loginValidator = new LoginRequestValidator();
            _authRepository = authRepository;
            _mapper = mapper;
            _config = config;
            _mailService = mailService;

        }

        public async Task<String> GetEmailBodyAsync(string email, string confirmationToken)
        {
            var body = new StringBuilder();
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
            body.Append($"<a href=\"http://localhost:4200/auth/confirm/email/{email}/{confirmationToken}\" style=\"background:#20e277;text-decoration:none !important; font-weight:500; margin-top:35px; color:#fff;text-transform:uppercase; font-size:14px;padding:10px 24px;display:inline-block;border-radius:50px;\">Confirm email</a>");
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
            return body.ToString();
        }

        public async Task<RegisterResponse> RegisterUserAsync(RegisterRequest registerRequest)
        {
            var validation = _registerValidator.Validate(registerRequest);
            var response = new RegisterResponse();
            if(!validation.IsValid)
            {
                foreach (var error in validation.Errors)
                {
                    response.ErrorMessages.Add(error.PropertyName, error.ErrorMessage);
                }
                response.Message = "Validation failed!";
                return response;
            }
            var registerRequestDTO = new RegisterRequestDTO();
            registerRequestDTO =_mapper.Map<RegisterRequestDTO>(registerRequest);
            var salt = RandomNumberGenerator.GetBytes(keySize);
            registerRequestDTO.Password = HashPasword(registerRequest.Password,salt);
            registerRequestDTO.PasswordSalt = Convert.ToHexString(salt);
            var confirmationToken = Guid.NewGuid().ToString();
            var succes = await _authRepository.RegisterUserAsync(registerRequestDTO,confirmationToken);
            if (!succes)
            {
                response.Message = "Registration failed! User already exists!";
                response.ErrorMessages.Add("User Exists", "There is already an entry with that email or phone number!");
                return response;
            }
            var mail = new MailRequest
            {
                Receiver = registerRequest.Email,
                Subject = "Email confirmation",
                Body = await GetEmailBodyAsync(registerRequest.Email, confirmationToken)
            };
            await _mailService.SendEmailAsync(mail);
            response.Message = "Registration succesfully!";
            response.Code = 200;
            response.Success = true;
            return response;
        }

        public async Task<LoginResponse> LoginUserAsync(LoginRequest loginRequest)
        {
            var response = new LoginResponse();
            var validation = _loginValidator.Validate(loginRequest);
            if (!validation.IsValid)
            {
                foreach (var error in validation.Errors)
                {
                    response.Message = "Validation failed!";
                    response.ErrorMessages.Add(error.PropertyName,error.ErrorMessage);
                }
                return response;
            }
            var exists = await _authRepository.CheckGmailAsync(loginRequest.Email);
            if(!exists.Exists)
            {
                response.Message = "User does not exists!";
                response.ErrorMessages.Add("User does not exists", "There is no user with that email!");
                return response;
            }
            var passwordAndSalt = await _authRepository.GetPasswordAndSaltAsync(loginRequest.Email);
            var salt = ConvertSaltToByte(passwordAndSalt.PasswordSalt);

            var validPassword = VerifyPassword(loginRequest.Password, passwordAndSalt.Password, salt);
            if (validPassword)
            {
                var verified = await _authRepository.IsEmailVerifiedAsync(loginRequest.Email);
                if (!verified)
                {
                    response.Message = "Email is not verified!";
                    return response;
                }
                response.Code = 200;
                response.Success = true;
                response.Token = GenerateToken(exists.ID);
                return response;
            }
            response.ErrorMessages.Add("Invalid", "User or password invalid!");
            response.Message = "User or password invalid!";
            return response;
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
        private bool VerifyPassword(string password, string hash, byte[] salt)
        {
            var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, hashAlgorithm, keySize);
            return CryptographicOperations.FixedTimeEquals(hashToCompare, Convert.FromHexString(hash));
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
        
        private string GenerateToken(int userID)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim("ID",userID.ToString())
            };
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(90),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<CheckGmailDTO> CheckGmailAsync(string email)
        {
            var exists = await _authRepository.CheckGmailAsync(email);
            return exists;
        }

        //TO DO : Implement this method
        //MODIFY PARAMETER TO INCLUDE A CLASS THAT HAS EMAIL AND FNAME LNAME AND ProfilePicURI SO YOU CAN REGISTER THE USER
        public async Task<LoginResponse> LogOrRegisterGmailUserAsync(RegisterRequestDTO req)
        {
            var exists = await CheckGmailAsync(req.Email);
            var response = new LoginResponse()
            {
                Code = 400,
                Success = false,
            };
            if(exists.Exists)
            {
                response.Code = 200;
                response.Success = true;
                response.Token = GenerateToken(exists.ID);
                return response;
            }
            else
            {
                await _authRepository.RegisterUserAsync(req,"");
                response.Code = 200;
                response.Success = true;
                response.Token = GenerateToken(exists.ID);
                return response;
            }
        }

        public async Task<bool> IsJwtValid(string token)
        {
            JwtValidator validator = new JwtValidator(_config);
            return validator.IsJwtValid(token);
        }

    }
}
