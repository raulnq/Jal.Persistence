using Jal.Persistence.Fluent.Interface;
using Jal.Persistence.Impl;
using Jal.Persistence.Interface;

namespace Jal.Persistence.Fluent.Impl
{
    public class RepositoryCommandFluentBuilder : IRepositoryCommandFluentBuilder
    {
        public IRepositoryCommand Create
        {
            get
            {
                return new RepositoryCommand();
            }
        }
    }
}