using System.Data;

namespace LinkedFit.PERSISTANCE.Repositories
{
    public interface IUnitOfWork
    {
        void Commit();
        void Dispose();
        void Rollback();

    }
}