using System;
using System.Data;
using Jal.Persistence.Interface;

namespace Jal.Persistence.Impl
{
    public class RepositoryConnection : IRepositoryConnection
    {
        public IDbConnection Connection { get; private set; }

        private readonly IRepositoryContext _parentRepositoryContext;

        public IRepositoryTransaction CurrentTransaction { get; set; }

        private readonly IRepositoryLogger _logger;

        public RepositoryConnection(IDbConnection connection, IRepositoryLogger logger, IRepositoryContext repositoryContext)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            if (repositoryContext == null)
            {
                throw new ArgumentNullException("repositoryContext");
            }

            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            Connection = connection;

            _parentRepositoryContext = repositoryContext;

            _logger = logger;
        }

        public IRepositoryTransaction BeginTransaction()
        {
            return BeginTransaction(IsolationLevel.ReadUncommitted);
        }

        public IRepositoryTransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            if (CurrentTransaction != null)
            {
                throw new Exception(string.Format("There is an active repository transaction for the connection, dispose it first. ConnectionId:{0}, TransactionId:{1}, IsolationLevel:{2}", Connection.GetHashCode(), CurrentTransaction.Transaction.GetHashCode(), CurrentTransaction.Transaction.IsolationLevel));
            }

            var transaction = Connection.BeginTransaction(isolationLevel);

            CurrentTransaction = new RepositoryTransaction(transaction, _logger, this);

            _logger.Info(string.Format("Transaction Opened. ConnectionId:{0}, TransactionId:{1}, IsolationLevel:{2}", Connection.GetHashCode(), Connection.GetHashCode(), CurrentTransaction.Transaction.IsolationLevel));

            return CurrentTransaction;
        }

        public void Close()
        {
            if (CurrentTransaction != null)
            {
                CurrentTransaction.Dispose();

                CurrentTransaction = null;
            }

            if (Connection != null)
            {
                Connection.Close();

                _logger.Info(string.Format("Connection closed. ConnectionId:{0}", Connection.GetHashCode()));
            }
        }

        public void Dispose()
        {
            Close();

            _logger.Info(string.Format("Connection disposed. ConnectionId:{0}", Connection.GetHashCode()));

            Connection.Dispose();

            Connection = null;

            _parentRepositoryContext.CurrentConnection = null;
        }

        private bool IsConnectionClosed()
        {
            return Connection.State == ConnectionState.Closed || Connection.State == ConnectionState.Broken;
        }

        public void Open()
        {
            if (IsConnectionClosed())
            {
                Connection.Open();

                _logger.Info(string.Format("Connection opened. ConnectionId:{0}", Connection.GetHashCode()));
            }
            else
            {
                throw new Exception(string.Format("The connection is already opened. ConnectionId:{0}", Connection.GetHashCode()));
            }
        }
    }
}