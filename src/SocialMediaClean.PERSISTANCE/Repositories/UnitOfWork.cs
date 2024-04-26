using LinkedFit.PERSISTANCE.Interfaces;
using SocialMediaClean.INFRASTRUCTURE.Interfaces;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;

namespace LinkedFit.PERSISTANCE.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        //protected readonly Guid _id;
        //protected readonly IDbConnectionFactory _db;


        //public UnitOfWork(IDbConnectionFactory db)
        //{
        //    _db = db;
        //    _id = Guid.NewGuid();
        //    _connection = CreateConnection();
        //    if (_connection.State != ConnectionState.Open)
        //    {
        //        _connection = CreateConnection();
        //    }
        //    _transaction = _connection.BeginTransaction();
        //}
        //public Guid Id => _id;
        //public IDbConnection Connection => _connection;
        //public IDbTransaction Transaction => _transaction;

        //public IDbConnection CreateConnection()
        //{
        //    var conn = _db.CreateDbConnection();
        //    return conn;
        //}

        //public virtual void BeginTransaction()
        //{
        //    _transaction = _connection.BeginTransaction();
        //}

        //public virtual void Commit()
        //{
        //    _transaction.Commit();
        //    _transaction.Dispose();
        //    BeginTransaction();
        //}
        //public virtual void Rollback()
        //{
        //    _transaction.Rollback();
        //    _transaction.Dispose();
        //    BeginTransaction();
        //}
        //public virtual void Dispose()
        //{
        //    if (_transaction != null)
        //    {
        //        _transaction.Dispose();
        //    }
        //    _transaction = null;
        //    _connection.Dispose();
        //    Dispose();
        //}

        //void IUnitOfWork.CreateConnectionAsync()
        //{
        //    throw new NotImplementedException();
        //}
        private IDbConnection _connection;
        private IDbTransaction _transaction;
        private IConfiguration _configuration;
        public UnitOfWork()
        {
            _configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            string connectionString = _configuration.GetConnectionString("SocialMediaClean");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException("Connection string is null");
            }
            _connection = new SqlConnection(connectionString);
            _connection.Open();
            _transaction = _connection.BeginTransaction();
        }

        public IDbConnection Connection => _connection;
        public IDbTransaction Transaction => _transaction;

        public void Rollback()
        {
            _transaction.Rollback();
        }

        public void Commit()
        {
            try
            {
                if (_transaction != null)
                {
                    return;
                }
                _transaction.Commit();
            }
            catch (Exception)
            {
                _transaction.Rollback();
                throw;
            }
            finally
            {
                if (_transaction is not null)
                {
                    _transaction.Dispose();
                    _transaction = _connection.BeginTransaction();
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_transaction is not null)
                {
                    _transaction.Dispose();
                    _transaction = null;
                }
                if (_connection is not null)
                {
                    _connection.Dispose();
                    _connection = null;
                }
            }
        }
        ~UnitOfWork()
        {
            Dispose(false);
        }

    }
}
