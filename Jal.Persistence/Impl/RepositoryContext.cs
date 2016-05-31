using System;
using Jal.Persistence.Fluent.Impl;
using Jal.Persistence.Fluent.Interface;
using Jal.Persistence.Interface;

namespace Jal.Persistence.Impl
{
    public class RepositoryContext : IRepositoryContext
    {
        public static IRepositoryContextStartFluentBuilder Builder
        {
            get { return new RepositoryContextFluenttBuilder(); }
        }

        public IRepositoryLogger Logger { get; set; }

        public RepositoryContext(IRepositoryDatabase repositoryDatabase)
        {
            Logger = AbstractRepositoryLogger.Instance;

            Database = repositoryDatabase;
        }

        public void Dispose()
        {
            Logger.Info("IRepositoryContext disposed");

            if (CurrentConnection!=null)
            {
                CurrentConnection.Dispose();

                CurrentConnection = null;
            }
        }

        public IRepositoryDatabase Database { get; private set; }

        public IRepositoryConnection CreateConnection()
        {
            if (CurrentConnection != null)
            {
                throw new Exception(string.Format("There is an active repository connection in the context, dispose it first. ConnectionId:{0}", CurrentConnection.Connection.GetHashCode()));
            }
            CurrentConnection = new RepositoryConnection(Database.CreateConnection(), Logger, this);

            return CurrentConnection;
        }

        public IRepositoryConnection CurrentConnection { get; set; }
    }
}
