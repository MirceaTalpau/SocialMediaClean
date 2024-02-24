using AutoMapper;
using LinkedFit.DOMAIN.Models.DTOs;
using Microsoft.IdentityModel.Tokens;
using SocialMediaClean.APPLICATION.Contracts;
using SocialMediaClean.APPLICATION.DTOs;
using SocialMediaClean.APPLICATION.Requests;
using SocialMediaClean.APPLICATION.Response;
using SocialMediaClean.APPLICATION.Validators;
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
                Body = $"Click <a href=\"http://localhost:4200/auth/confirm/email/{registerRequest.Email}/{confirmationToken}\">here</a> to confirm your email."
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

    }
}
