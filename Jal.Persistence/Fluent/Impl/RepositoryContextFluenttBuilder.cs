using System;
using Jal.Persistence.Fluent.Interface;
using Jal.Persistence.Impl;
using Jal.Persistence.Interface;

namespace Jal.Persistence.Fluent.Impl
{
    public class RepositoryContextFluenttBuilder : IRepositoryContextStartFluentBuilder, IRepositoryContextFluentBuilder
    {

        private IRepositoryDatabase _repositoryDatabase;

        private IRepositoryContext _repositoryContext;

        private IRepositoryLogger _repositoryLogger;

        public IRepositoryContextFluentBuilder UseDatabase(IRepositoryDatabase repositoryDatabase)
        {
            if (repositoryDatabase == null)
            {
                throw new ArgumentNullException("repositoryDatabase");
            }
            _repositoryDatabase = repositoryDatabase;
            return this;
        }

        public IRepositoryContextEndFluentBuilder UseRepositoryContext(IRepositoryContext repositoryContext)
        {
            if (repositoryContext == null)
            {
                throw new ArgumentNullException("repositoryContext");
            }
            _repositoryContext = repositoryContext;
            return this;
        }

        public IRepositoryContext Create
        {
            get
            {
                if (_repositoryContext != null)
                {
                    return _repositoryContext;
                }

                var result = new RepositoryContext(_repositoryDatabase);

                if (_repositoryLogger!=null)
                {
                    result.Logger = _repositoryLogger;
                }

                return result;
            }
        }

        public IRepositoryContextFluentBuilder UseLogger(IRepositoryLogger repositoryLogger)
        {
            if (repositoryLogger == null)
            {
                throw new ArgumentNullException("repositoryLogger");
            }
            _repositoryLogger = repositoryLogger;
            return this;
        }
    }
}