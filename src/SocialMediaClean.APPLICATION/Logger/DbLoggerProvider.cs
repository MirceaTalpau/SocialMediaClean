using SocialMediaClean.INFRASTRUCTURE.Interfaces;
using SocialMediaClean.APPLICATION.Logger;


namespace SocialMediaClean.APPLICATION.Logger
{
    public class DbLoggerProvider : ILoggerProvider
    {
        private readonly string _connectionString;
        private readonly IDbConnectionFactory _connectionFactory;

        public DbLoggerProvider(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new DbLogger(_connectionFactory);
        }

        public void Dispose()
        {
        }
    }
}
