using Jal.Persistence.Interface;

namespace Jal.Persistence.Fluent.Interface
{
    public interface IRepositoryDatabaseEndFluentBuilder
    {
        IRepositoryDatabase Create
        {
            get;
        }
    }
}