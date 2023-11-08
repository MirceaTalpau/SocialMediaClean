using AutoFixture;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shouldly;
using SocialMediaClean.APPLICATION.Mapper;
using SocialMediaClean.APPLICATION.Requests;
using SocialMediaClean.APPLICATION.Services;
using SocialMediaClean.DOMAIN.Enums;
using SocialMediaClean.INFRASTRUCTURE.Implementation;
using SocialMediaClean.INFRASTRUCTURE.Interfaces;
using SocialMediaClean.INFRASTRUCTURE.Settings;
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
        private readonly MailService _mailService;
        private IServiceProvider _serviceProvider;



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
            var services = new ServiceCollection();
            services.Configure<MailSettings>(_config.GetSection("MailSettings"));
            _serviceProvider = services.BuildServiceProvider();
            var mailSettingsOptions = _serviceProvider.GetRequiredService<IOptions<MailSettings>>();

            _mailService = new MailService(mailSettingsOptions);
            _authService = new AuthService(_auth, _mapper,_config,_mailService);
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
            response.Success.ShouldBeTrue();
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
            response.Success.ShouldBeFalse();
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
            response.Success.ShouldBeFalse();
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
            response.Success.ShouldBeFalse();
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
            response.Success.ShouldBeFalse();
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
            response.Success.ShouldBeFalse();
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
            response.Success.ShouldBeFalse();
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
            response.Success.ShouldBeFalse();
        }
        [Fact]
        public async Task LoginUserAsync_WhenUserIsCorrect_ReturnsToken()
        {
            //ARRANGE
            var login = new LoginRequest
            {
                Email = "talpaumirceacristian@gmail.com",
                Password = "ceva123"
            };
            //ACT
            var response = await _authService.LoginUserAsync(login);
            //ASSERT
            response.ErrorMessages.Count.ShouldBe(0);
            response.Token.ShouldNotBeNullOrEmpty();
        }
        [Fact]
        public async Task LoginUserAsync_WhenUserNotExists_ReturnsFalse()
        {
            //ARRANGE
            var login = new LoginRequest
            {
                Email = "talpaumirceacristian@gmail.com",
                Password = "passsword"
            };
            //ACT
            var response = await _authService.LoginUserAsync(login);
            //ASSERT
            response.ErrorMessages.Count.ShouldBe(1);
            response.Token.ShouldBeNullOrEmpty();
        }

        [Fact]
        public async Task LoginUserAsync_WhenEmailIsNotVerified_ReturnsFalse()
        {
            //ARRANGE
            var login = new LoginRequest
            {
                Email = "talpaumircea123@gmail.com",
                Password = "ceva123"
            };
            //ACT
            var response = await _authService.LoginUserAsync(login);
            //ASSERT
            response.Success.ShouldBeFalse();
            response.Token.ShouldBeNullOrEmpty();
        }
    }
}