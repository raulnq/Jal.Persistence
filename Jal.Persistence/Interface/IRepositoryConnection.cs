using System;
using System.Data;

namespace Jal.Persistence.Interface
{
    public interface IRepositoryConnection : IDisposable
    {
        IDbConnection Connection { get; }

        IRepositoryTransaction CurrentTransaction { get; set; }

        IRepositoryTransaction BeginTransaction(IsolationLevel isolationLevel);

        IRepositoryTransaction BeginTransaction();

        void Open();

        void Close();
    }
}