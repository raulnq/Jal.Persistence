using Jal.Persistence.Interface;

namespace Jal.Persistence.Fluent.Interface
{
    public interface IRepositoryDatabaseStartFluentBuilder
    {
        IRepositoryDatabaseFluentBuilder UseSettings(IRepositorySettings repositorySettings);

        IRepositoryDatabaseEndFluentBuilder UseRepositoryDatabase(IRepositoryDatabase repositoryDatabase);
    }
}