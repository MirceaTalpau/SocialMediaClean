using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using SocialMediaClean.APPLICATION.Contracts;
using SocialMediaClean.APPLICATION.DTOs;
using SocialMediaClean.APPLICATION.Requests;
using SocialMediaClean.APPLICATION.Response;
using SocialMediaClean.APPLICATION.Validators;
using SocialMediaClean.DOMAIN.Models.DTOs;
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
        const int keySize = 64;
        const int iterations = 350000;
        HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;

        public AuthService(IAuthRepository authRepository,IMapper mapper, IConfiguration config)
        {
            _registerValidator = new RegisterRequestValidator();
            _loginValidator = new LoginRequestValidator();
            _authRepository = authRepository;
            _mapper = mapper;
            _config = config;

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
                response.Message = "Registration failed!";
                return response;
            }
            var registerRequestDTO = new RegisterRequestDTO();
            registerRequestDTO =_mapper.Map<RegisterRequestDTO>(registerRequest);
            var salt = RandomNumberGenerator.GetBytes(keySize);
            registerRequestDTO.Password = HashPasword(registerRequest.Password,salt);
            registerRequestDTO.PasswordSalt = Convert.ToHexString(salt);
            var succes = await _authRepository.RegisterUserAsync(registerRequestDTO);
            if (!succes)
            {
                response.Message = "Registration failed!";
                response.ErrorMessages.Add("User Exists", "There is already an entry with that email or phone number!");
                return response;
            }
            response.Message = "Registration succesfully!";
            response.Succes = true;
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
                    response.ErrorMessages.Add(error.PropertyName,error.ErrorMessage);
                }
                return response;
            }
            var passwordAndSalt = await _authRepository.GetPasswordAndSaltAsync(loginRequest.Email);
            var salt = ConvertSaltToByte(passwordAndSalt.PasswordSalt);

            //var salt = Convert.ToByte(passwordAndSalt.PasswordSalt);
            if (VerifyPassword(loginRequest.Password, passwordAndSalt.Password, salt))
            {
                response.Token = GenerateToken(passwordAndSalt.ID);
                response.Success = true;
                return response;
            }
            response.ErrorMessages.Add("Invalid", "User or password invalid!");
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
                expires: DateTime.Now.AddMinutes(90),
                signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
