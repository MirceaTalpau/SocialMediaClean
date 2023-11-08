using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SocialMediaClean.INFRASTRUCTURE.Implementation;
using SocialMediaClean.INFRASTRUCTURE.Interfaces;
using SocialMediaClean.INFRASTRUCTURE.Models;
using SocialMediaClean.INFRASTRUCTURE.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace SocialMediaClean.IntegrationTests
{
    public class EmailServiceTests
    {
        private readonly IMailService _mailService;
        private readonly IConfiguration _config;
        private IServiceProvider _serviceProvider;
        private readonly IOptions<MailSettings> mailSettings;
        public EmailServiceTests()
        {
            _config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var services = new ServiceCollection();
            services.Configure<MailSettings>(_config.GetSection("MailSettings"));
            _serviceProvider = services.BuildServiceProvider();
            var mailSettingsOptions = _serviceProvider.GetRequiredService<IOptions<MailSettings>>();

            _mailService = new MailService(mailSettingsOptions);
        }
        [Fact]
        public async Task SendEmailAsync_WhenEmailIsCorrect()
        {
            //ARRANGE
            var mailRequest = new MailRequest
            {
                Receiver = "talpaumirceacristian@gmail.com",
                Subject = "test",
                Body = "blablalbablbalbal"
            };
            //ACT
            await _mailService.SendEmailAsync(mailRequest);
            //ASSERT

        }
    }
}
