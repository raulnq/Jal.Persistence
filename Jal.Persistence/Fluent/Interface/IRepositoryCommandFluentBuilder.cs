using Jal.Persistence.Interface;

namespace Jal.Persistence.Fluent.Interface
{
    public interface IRepositoryCommandFluentBuilder
    {
        IRepositoryCommand Create
        {
            get;
        }
    }
}