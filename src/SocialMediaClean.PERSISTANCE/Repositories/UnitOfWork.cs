using LinkedFit.PERSISTANCE.Interfaces;
using SocialMediaClean.INFRASTRUCTURE.Interfaces;
using System.Data;
using System.Data.SqlClient;

namespace LinkedFit.PERSISTANCE.Repositories
{
    public class UnitOfWork : IUnitOfWork , IDisposable
    {
        protected readonly Guid _id;
        protected readonly IDbConnectionFactory _db;
        protected IDbConnection _connection;
        protected IDbTransaction _transaction;

        public UnitOfWork(IDbConnectionFactory db)
        {
            _db = db;
            _id = Guid.NewGuid();
            _connection = CreateConnection();
            if (_connection.State != ConnectionState.Open)
            {
                _connection = CreateConnection();
            }
            _transaction = _connection.BeginTransaction();
        }
        public Guid Id => _id;
        public IDbConnection Connection => _connection;
        public IDbTransaction Transaction => _transaction;

        public IDbConnection CreateConnection()
        {
            var conn = _db.CreateDbConnection();
            return conn;
        }

        public virtual void BeginTransaction()
        {
            _transaction = _connection.BeginTransaction();
        }

        public virtual void Commit()
        {
            _transaction.Commit();
            _transaction.Dispose();
            BeginTransaction();
        }
        public virtual void Rollback()
        {
            _transaction.Rollback();
            _transaction.Dispose();
            BeginTransaction();
        }
        public virtual void Dispose()
        {
            if (_transaction != null)
            {
                _transaction.Dispose();
            }
            _transaction = null;
            _connection.Dispose();
            Dispose();
        }

        void IUnitOfWork.CreateConnectionAsync()
        {
            throw new NotImplementedException();
        }
    }
}
