using Dapper;
using Formatting = Newtonsoft.Json.Formatting;
using SocialMediaClean.INFRASTRUCTURE.Interfaces;
using System.Data;
using Newtonsoft.Json;


namespace SocialMediaClean.APPLICATION.Logger
{
    public class DbLogger : ILogger
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private const string INSERT_LOG = "usp_InsertLog";
        public DbLogger(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public async void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            var message = formatter(state, exception);
            var serializedMessage = JsonConvert.SerializeObject(state,Formatting.Indented);
            var logEntry = new
            {
                Message = message,
                LogLevel = logLevel.ToString()
            };
            using (var db = await _connectionFactory.CreateDbConnectionAsync())
            {
                var parameters = new DynamicParameters();
                parameters.Add("Message", logEntry.Message);
                parameters.Add("LogLevel", logEntry.LogLevel);
                await db.ExecuteAsync(INSERT_LOG, parameters, commandType: CommandType.StoredProcedure);
                

            }
        }
    }
}
