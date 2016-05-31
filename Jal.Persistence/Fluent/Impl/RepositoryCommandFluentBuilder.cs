using System;
using Jal.Persistence.Fluent.Interface;
using Jal.Persistence.Impl;
using Jal.Persistence.Interface;

namespace Jal.Persistence.Fluent.Impl
{
    public class RepositoryCommandFluentBuilder : IRepositoryCommandStartFluentBuilder
    {
        private IRepositoryLogger _repositoryLogger;

        private IRepositoryCommand _repositoryCommand;

        public IRepositoryCommand Create
        {
            get
            {
                if (_repositoryCommand != null)
                {
                    return _repositoryCommand;
                }

                var result = new RepositoryCommand();

                if (_repositoryLogger != null)
                {
                    result.Logger = _repositoryLogger;
                }

                return result;
            }
        }

        public IRepositoryCommandEndFluentBuilder UseLogger(IRepositoryLogger repositoryLogger)
        {
            if (repositoryLogger == null)
            {
                throw new ArgumentNullException("repositoryLogger");
            }
            _repositoryLogger  = repositoryLogger;
            return this;
        }

        public IRepositoryCommandEndFluentBuilder UseRepositoryCommand(IRepositoryCommand repositoryCommand)
        {
            if (repositoryCommand == null)
            {
                throw new ArgumentNullException("repositoryCommand");
            }
            _repositoryCommand = repositoryCommand;
            return this;
        }
    }
}