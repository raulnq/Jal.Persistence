using System;

namespace Jal.Persistence.Interface
{
    public interface IRepositoryContext : IDisposable
    {
        IRepositoryDatabase Database { get; }

        IRepositoryLogger Logger { get; set; }

        IRepositoryConnection CreateConnection();

        IRepositoryConnection CurrentConnection { get; set; }
    }
}