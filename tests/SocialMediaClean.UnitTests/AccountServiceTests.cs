using Microsoft.Extensions.Configuration;
using Moq;
using Shouldly;
using SocialMediaClean.INFRASTRUCTURE.Implementation;
using SocialMediaClean.INFRASTRUCTURE.Interfaces;
using SocialMediaClean.PERSISTANCE.Interfaces;
using SocialMediaClean.PERSISTANCE.Repositories;


namespace SocialMediaClean.UnitTests
{
    public class AccountServiceTests
    {
        private readonly Mock<IAccountRepository> _accountRepo;
        private readonly Mock<DbConnectionFactory> _db;
        private readonly IConfiguration _config;
        public AccountServiceTests()
        {
            _config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            //var _config = new Mock<IConfiguration>();
            _db = new Mock<DbConnectionFactory>(_config);
            _accountRepo = new Mock<IAccountRepository>();
            
            
        }
        [Fact]
        public async Task Test1()
        {
            string idResult = "2";
            _accountRepo.Setup(x => x.CheckExistingUserAsync("cevasdadas")).ReturnsAsync(idResult);
            var id = await _accountRepo.Object.CheckExistingUserAsync("cevasdadas");

            id.ShouldBe(idResult);
        }
    }
}