using System.Data.SqlClient;

namespace SocialMediaClean.INFRASTRUCTURE.Interfaces
{
    public interface IDbConnectionFactory
    {
        Task<SqlConnection> CreateDbConnectionAsync();
    }
}
