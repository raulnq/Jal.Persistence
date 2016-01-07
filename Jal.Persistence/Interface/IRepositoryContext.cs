using System;

namespace Jal.Persistence.Interface
{
    public interface IRepositoryContext : IDisposable
    {
        IRepositoryDatabase Database { get; }

        IRepositoryCommand Command { get; }

        IRepositoryConnection CreateConnection();

        IRepositoryConnection CurrentRepositoryConnection { get; set; }
    }
}