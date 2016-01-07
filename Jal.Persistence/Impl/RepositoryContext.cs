using System;
using Jal.Persistence.Interface;

namespace Jal.Persistence.Impl
{
    public class RepositoryContext : IRepositoryContext
    {
        private readonly IRepositoryLogger _logger;

        public RepositoryContext(IRepositoryLogger log, IRepositoryDatabase repositoryDatabase, IRepositoryCommand repositoryCommand)
        {
            _logger = log;
            Database = repositoryDatabase;
            Command = repositoryCommand;
        }

        public void Dispose()
        {
            _logger.Info("IRepositoryContext disposed");

            if (CurrentRepositoryConnection!=null)
            {
                CurrentRepositoryConnection.Dispose();

                CurrentRepositoryConnection = null;
            }
        }

        public IRepositoryDatabase Database { get; private set; }

         public IRepositoryCommand Command { get; private set; }

        public IRepositoryConnection CreateConnection()
        {
            if (CurrentRepositoryConnection != null)
            {
                throw new Exception(string.Format("There is an active repository connection in the context, dispose it first. ConnectionId:{0}", CurrentRepositoryConnection.Connection.GetHashCode()));
            }
            CurrentRepositoryConnection = new RepositoryConnection(Database.CreateConnection(), _logger, this);

            return CurrentRepositoryConnection;
        }

        public IRepositoryConnection CurrentRepositoryConnection { get; set; }
    }
}
