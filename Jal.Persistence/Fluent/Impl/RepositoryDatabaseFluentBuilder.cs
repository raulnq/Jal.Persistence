using System;
using Jal.Persistence.Fluent.Interface;
using Jal.Persistence.Impl.Sql;
using Jal.Persistence.Interface;

namespace Jal.Persistence.Fluent.Impl
{
    public class RepositoryDatabaseFluentBuilder : IRepositoryDatabaseStartFluentBuilder, IRepositoryDatabaseFluentBuilder
    {
        private IRepositorySettings _repositorySettings;

        private IRepositoryDatabase _repositoryDatabase;

        private IRepositoryLogger _repositoryLogger;

        public IRepositoryDatabaseFluentBuilder UseSettings(IRepositorySettings repositorySettings)
        {
            if (repositorySettings == null)
            {
                throw new ArgumentNullException("repositorySettings");
            }

            _repositorySettings = repositorySettings;

            return this;
        }

        public IRepositoryDatabaseEndFluentBuilder UseRepositoryDatabase(IRepositoryDatabase repositoryDatabase)
        {
            if (repositoryDatabase == null)
            {
                throw new ArgumentNullException("repositoryDatabase");
            }

            _repositoryDatabase = repositoryDatabase;

            return this;
        }

        public IRepositoryDatabaseEndFluentBuilder UseLogger(IRepositoryLogger repositoryLogger)
        {
            if (repositoryLogger == null)
            {
                throw new ArgumentNullException("repositoryLogger");
            }

            _repositoryLogger = repositoryLogger;

            return this;
        }

        public IRepositoryDatabase Create
        {
            get
            {
                if (_repositoryDatabase != null)
                {
                    return _repositoryDatabase;
                }

                if (_repositorySettings==null)
                {
                    throw new Exception("An implementation of IRepositorySettings is needed");
                }

                var result = new RepositoryDatabase(_repositorySettings);

                if (_repositoryLogger != null)
                {
                    result.Logger = _repositoryLogger;
                }

                return result;
            }
        }


    }
}