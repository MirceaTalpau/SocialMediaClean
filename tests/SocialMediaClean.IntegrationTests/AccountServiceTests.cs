using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shouldly;
using SocialMediaClean.APPLICATION.Services;
using SocialMediaClean.INFRASTRUCTURE.Implementation;
using SocialMediaClean.INFRASTRUCTURE.Interfaces;
using SocialMediaClean.INFRASTRUCTURE.Settings;
using SocialMediaClean.PERSISTANCE.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaClean.IntegrationTests
{
    public class AccountServiceTests
    {
        private readonly AccountRepository _accountRepository;
        private readonly MailService _mailService;
        private readonly IDbConnectionFactory _db;
        private readonly IConfiguration _config;
        private IServiceProvider _serviceProvider;
        private readonly IOptions<MailSettings> mailSettings;
        private readonly AccountService _accountService;
        public AccountServiceTests()
        {
            _config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var services = new ServiceCollection();
            services.Configure<MailSettings>(_config.GetSection("MailSettings"));
            _serviceProvider = services.BuildServiceProvider();
            var mailSettingsOptions = _serviceProvider.GetRequiredService<IOptions<MailSettings>>();

            _mailService = new MailService(mailSettingsOptions);
            _config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            _db = new DbConnectionFactory(_config);
            _accountRepository = new AccountRepository(_db);
            _accountService = new AccountService(_accountRepository,_mailService,_config);

        }

        [Fact]
        public async Task SendPasswordResetMailAsync_WhenMailIsValid_ReturnsTrue()
        {
            //ARRANGE
            var email = "talpaumirceacristian@gmail.com";
            //ACT
            var response = await _accountService.SendPasswordResetMailAsync(email);
            //ASSERT
            response.Success.ShouldBeTrue();
        }
        [Fact]
        public async Task SendPasswordResetMailAsync_WhenMailIsValidAndNotConfirmed_ReturnsFalse()
        {
            //ARRANGE
            var email = "talpaumircea@gmail.com";
            //ACT
            var response = await _accountService.SendPasswordResetMailAsync(email);
            //ASSERT
            response.Success.ShouldBeFalse();
        }
        [Fact]
        public async Task SendPasswordResetMailAsync_WhenMailNotExists_ReturnsFalse()
        {
            //ARRANGE
            var email = "talpaumirceacristiannnnn@gmail.com";
            //ACT
            var response = await _accountService.SendPasswordResetMailAsync(email);
            //ASSERT
            response.Success.ShouldBeFalse();
        }
        [Fact]
        public async Task ResendEmailConfirmationTokenAsync_WhenMailIsValidAndConfirmed_ReturnsFalse()
        {
            //ARRANGE
            var email = "talpaumirceacristian@gmail.com";
            //ACT
            var response = await _accountService.ResendEmailConfirmationTokenAsync(email);
            //ASSERT
            response.Success.ShouldBeFalse();
        }
        [Fact]
        public async Task ResendEmailConfirmationTokenAsync_WhenMailIsValidAndNotConfirmed_ReturnsTrue()
        {
            //ARRANGE
            var email = "talpaumircea@gmail.com";
            //ACT
            var response = await _accountService.ResendEmailConfirmationTokenAsync(email);
            //ASSERT
            response.Success.ShouldBeTrue();
        }
        [Fact]
        public async Task ResendEmailConfirmationTokenAsync_WhenEmailNotExists_ReturnsFalse()
        {
            //ARRANGE
            var email = "dasdsadasdsadasjdhajksdhaskdahsjkdhqiuwehiqw@gmail.com";
            //ACT
            var response = await _accountService.ResendEmailConfirmationTokenAsync(email);
            //ASSERT
            response.Success.ShouldBeFalse();
        }
    }
}
