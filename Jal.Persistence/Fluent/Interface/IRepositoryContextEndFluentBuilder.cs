using Jal.Persistence.Interface;

namespace Jal.Persistence.Fluent.Interface
{
    public interface IRepositoryContextEndFluentBuilder
    {
        IRepositoryContext Create { get; }
    }
}