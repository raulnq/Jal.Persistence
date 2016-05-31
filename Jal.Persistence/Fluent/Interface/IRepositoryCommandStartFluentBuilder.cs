using Jal.Persistence.Interface;

namespace Jal.Persistence.Fluent.Interface
{
    public interface IRepositoryCommandStartFluentBuilder : IRepositoryCommandFluentBuilder
    {
        IRepositoryCommandEndFluentBuilder UseRepositoryCommand(IRepositoryCommand repositoryCommand);
    }
}