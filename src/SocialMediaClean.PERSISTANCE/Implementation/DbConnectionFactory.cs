using SocialMediaClean.INFRASTRUCTURE.Interfaces;
using System.Data.SqlClient;

namespace SocialMediaClean.INFRASTRUCTURE.Implementation
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly IConfiguration _configuration;
        public DbConnectionFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<SqlConnection> CreateDbConnectionAsync()
        {
            var connectionString = _configuration.GetConnectionString("SocialMediaClean");
            var conn = new SqlConnection(connectionString);
            await conn.OpenAsync();
            return conn;      
        }
        public SqlConnection CreateDbConnection()
        {
            var connectionString = _configuration.GetConnectionString("SocialMediaClean");
            var conn = new SqlConnection(connectionString);
            conn.Open();
            return conn;
        }
    }
}
