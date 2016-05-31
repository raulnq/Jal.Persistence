using Jal.Persistence.Impl;
using Jal.Persistence.Interface;

namespace Jal.Persistence.Fluent.Interface
{
    public interface IRepositoryContextFluentBuilder : IRepositoryContextEndFluentBuilder
    {
        IRepositoryContextFluentBuilder UseLogger(IRepositoryLogger repositoryLogger);
    }
}