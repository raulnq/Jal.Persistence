using Jal.Persistence.Impl;
using Jal.Persistence.Interface;

namespace Jal.Persistence.Fluent.Interface
{
    public interface IRepositoryContextStartFluentBuilder
    {
        IRepositoryContextFluentBuilder UseDatabase(IRepositoryDatabase repositoryDatabase);

        IRepositoryContextEndFluentBuilder UseRepositoryContext(IRepositoryContext repositoryContext);
    }
}