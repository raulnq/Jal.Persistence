using Jal.Persistence.Interface;

namespace Jal.Persistence.Fluent.Interface
{
    public interface IRepositoryDatabaseFluentBuilder : IRepositoryDatabaseEndFluentBuilder
    {
        IRepositoryDatabaseEndFluentBuilder UseLogger(IRepositoryLogger repositoryLogger);
    }
}