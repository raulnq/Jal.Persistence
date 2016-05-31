using Jal.Persistence.Interface;

namespace Jal.Persistence.Fluent.Interface
{
    public interface IRepositoryCommandFluentBuilder : IRepositoryCommandEndFluentBuilder
    {
        IRepositoryCommandEndFluentBuilder UseLogger(IRepositoryLogger repositoryLogger);
    }
}