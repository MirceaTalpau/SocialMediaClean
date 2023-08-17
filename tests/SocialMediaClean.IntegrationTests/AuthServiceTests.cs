using AutoFixture;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Shouldly;
using SocialMediaClean.APPLICATION.Mapper;
using SocialMediaClean.APPLICATION.Requests;
using SocialMediaClean.APPLICATION.Services;
using SocialMediaClean.DOMAIN.Enums;
using SocialMediaClean.INFRASTRUCTURE.Implementation;
using SocialMediaClean.INFRASTRUCTURE.Interfaces;
using SocialMediaClean.PERSISTANCE.Repositories;
using System.Net.Mail;

namespace SocialMediaClean.IntegrationTests
{
    public class AuthServiceTests
    {
        private readonly IDbConnectionFactory _db;
        private readonly IConfiguration _config;
        private readonly AuthRepository _auth;
        private readonly IFixture _fixture;
        private readonly AuthService _authService;
        private readonly IMapper _mapper;


        public AuthServiceTests()
        {
            _config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            _db = new DbConnectionFactory(_config);
            _auth = new AuthRepository(_db);
            _fixture = new Fixture();
            var mapperConfig = new MapperConfiguration(cfg => {
                cfg.AddProfile<MapperProfile>();
            });
            _mapper = mapperConfig.CreateMapper();
            _authService = new AuthService(_auth, _mapper,_config);
        }
        
        [Fact]
        public async Task RegisterUserAsync_WhenDataIsValidAndUserNotExists_ReturnsTrue()
        {
            //ARRANGE
            var user = _fixture.Build<RegisterRequest>()
                .With(x => x.Gender, Gender.M)
                .With(x => x.Email, _fixture.Create<MailAddress>().Address)
                .Create();
            //ACT
            var response =await _authService.RegisterUserAsync(user);
            //ASSERT
            response.ErrorMessages.ShouldBeEmpty();
            response.Succes.ShouldBeTrue();
        }
        [Fact]
        public async Task RegisterUserAsync_WhenDataIsValidAndUserExists_ReturnsFalse()
        {
            //ARRANGE
            var user = _fixture.Build<RegisterRequest>()
                .With(x => x.Gender, Gender.M)
                .With(x => x.Email, "talpaumircea@gmail.com")
                .With(x => x.PhoneNumber, "1234567890")
                .Create();
            //ACT
            var response = await _authService.RegisterUserAsync(user);
            //ASSERT
            response.ErrorMessages.Count.ShouldBe(1);
            response.Succes.ShouldBeFalse();
        }
        [Fact]
        public async Task RegisterUserAsync_WhenEmailNotValid_ReturnsFalse()
        {
            //ARRANGE
            var user = _fixture.Build<RegisterRequest>()
                .With(x => x.Gender, Gender.M)
                .With(x => x.Email, "talpaumircea")
                .With(x => x.PhoneNumber, "1234567890")
                .Create();
            //ACT
            var response = await _authService.RegisterUserAsync(user);
            //ASSERT
            response.ErrorMessages.Count.ShouldBe(1);
            response.Succes.ShouldBeFalse();
        }
        [Fact]
        public async Task RegisterUserAsync_WhenFirstNameMissing_ReturnsFalse()
        {
            //ARRANGE
            var user = _fixture.Build<RegisterRequest>()
                .With(x => x.Gender, Gender.M)
                .With(x => x.Email, "talpaumircea@gmail.com")
                .With(x => x.PhoneNumber, "1234567890")
                .With(x => x.FirstName, "")
                .Create();
            //ACT
            var response = await _authService.RegisterUserAsync(user);
            //ASSERT
            response.ErrorMessages.Count.ShouldBe(1);
            response.Succes.ShouldBeFalse();
        }
        [Fact]
        public async Task RegisterUserAsync_WhenLastNameMissing_ReturnsFalse()
        {
            //ARRANGE
            var user = _fixture.Build<RegisterRequest>()
                .With(x => x.Gender, Gender.M)
                .With(x => x.Email, "talpaumircea@gmail.com")
                .With(x => x.PhoneNumber, "1234567890")
                .With(x => x.LastName, "")
                .Create();
            //ACT
            var response = await _authService.RegisterUserAsync(user);
            //ASSERT
            response.ErrorMessages.Count.ShouldBe(1);
            response.Succes.ShouldBeFalse();
        }
        [Fact]
        public async Task RegisterUserAsync_WhenPasswordMissing_ReturnsFalse()
        {
            //ARRANGE
            var user = _fixture.Build<RegisterRequest>()
                .With(x => x.Gender, Gender.M)
                .With(x => x.Email, "talpaumircea@gmail.com")
                .With(x => x.PhoneNumber, "1234567890")
                .With(x => x.Password, "")
                .Create();
            //ACT
            var response = await _authService.RegisterUserAsync(user);
            //ASSERT
            response.ErrorMessages.Count.ShouldBe(1);
            response.Succes.ShouldBeFalse();
        }
        [Fact]
        public async Task RegisterUserAsync_WhenBirthdayMissing_ReturnsFalse()
        {
            //ARRANGE
            var user = _fixture.Build<RegisterRequest>()
                .With(x => x.Gender, Gender.M)
                .With(x => x.Email, "talpaumircea@gmail.com")
                .With(x => x.PhoneNumber, "1234567890")
                .With(x => x.BirthDay, DateTime.MinValue)
                .Create();
            //ACT
            var response = await _authService.RegisterUserAsync(user);
            //ASSERT
            response.ErrorMessages.Count.ShouldBe(1);
            response.Succes.ShouldBeFalse();
        }
        [Fact]
        public async Task RegisterUserAsync_WhenGenderIncorrect_ReturnsFalse()
        {
            //ARRANGE
            var user = _fixture.Build<RegisterRequest>()
                .With(x => x.Gender, Gender.M)
                .With(x => x.Email, "talpaumircea@gmail.com")
                .With(x => x.PhoneNumber, "1234567890")
                .With(x => x.Gender, "A")
                .Create();
            //ACT
            var response = await _authService.RegisterUserAsync(user);
            //ASSERT
            response.ErrorMessages.Count.ShouldBe(1);
            response.Succes.ShouldBeFalse();
        }
        [Fact]
        public async Task LoginUserAsync_WhenUserIsCorrect_ReturnsToken()
        {
            var login = new LoginRequest
            {
                Email = "talpaumirceacristian@gmail.com",
                Password = "password"
            };
            var response = await _authService.LoginUserAsync(login);
            response.ErrorMessages.ShouldBeEmpty();
            response.Token.ShouldNotBeNullOrEmpty();
        }
        [Fact]
        public async Task LoginUserAsync_WhenUserNotExists_ReturnsFalse()
        {
            var login = new LoginRequest
            {
                Email = "talpaumirceacristian@gmail.com",
                Password = "passsword"
            };
            var response = await _authService.LoginUserAsync(login);
            response.ErrorMessages.Count.ShouldBe(1);
            response.Token.ShouldBeNullOrEmpty();
        }
    }
}