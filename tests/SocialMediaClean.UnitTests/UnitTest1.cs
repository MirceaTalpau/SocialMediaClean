using Microsoft.Extensions.Configuration;
using SocialMediaClean.INFRASTRUCTURE.Implementation;
using SocialMediaClean.INFRASTRUCTURE.Interfaces;
using System.Data.SqlClient;


namespace SocialMediaClean.UnitTests
{
    public class UnitTest1
    {
        private readonly IDbConnectionFactory _db;
        private readonly IConfiguration _config;
        public UnitTest1()
        {
            _config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            _db = new DbConnectionFactory(_config);
        }
        [Fact]
        public void Test1()
        {
            var db = _db.CreateDbConnectionAsync();
            Assert.NotNull(db);
        }
    }
}