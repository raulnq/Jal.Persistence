using System;
using System.Data;

namespace Jal.Persistence.Interface
{
    public interface IRepositoryTransaction : IDisposable
    {
        IDbTransaction Transaction { get; }

        void Commit();

        void Rollback();
    }
}