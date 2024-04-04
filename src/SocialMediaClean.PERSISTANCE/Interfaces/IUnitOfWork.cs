using System.Data;

namespace LinkedFit.PERSISTANCE.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IDbConnection Connection { get; }
        Guid Id { get; }
        IDbTransaction Transaction { get; }

        void BeginTransaction();
        void Commit();
        void CreateConnectionAsync();
        IDbConnection CreateConnection();

        void Dispose();
        void Rollback();
    }
}