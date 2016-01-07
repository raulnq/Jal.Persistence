using System;
using System.Data;
using Jal.Persistence.Interface;

namespace Jal.Persistence.Impl
{
    public class RepositoryTransaction : IRepositoryTransaction
    {
        private readonly IRepositoryLogger _logger;

        private readonly IRepositoryConnection _parentRepositoryConnection;

        public IDbTransaction Transaction { get; private set; }

        private bool IsCommitted()
        {
            return _committed;
        }

        private bool _committed;

        public RepositoryTransaction(IDbTransaction transaction, IRepositoryLogger logger, IRepositoryConnection repositoryConnection)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }

            if (repositoryConnection == null)
            {
                throw new ArgumentNullException("repositoryConnection");
            }

            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            Transaction = transaction;

            _logger = logger;

            _committed = false;

            _parentRepositoryConnection = repositoryConnection;
        }

        private bool IsConnectionClosed()
        {
            return _parentRepositoryConnection.Connection.State == ConnectionState.Closed || _parentRepositoryConnection.Connection.State == ConnectionState.Broken;
        }

        public void Commit()
        {
            if (IsCommitted())
            {
                throw new Exception(string.Format("Transaction is already committed. ConnectionId:{0}", _parentRepositoryConnection.Connection.GetHashCode()));
            }

            try
            {
                _logger.Info(string.Format("Transaction Commited. ConnectionId:{0}, TransactionId:{1}, IsolationLevel:{2}", _parentRepositoryConnection.Connection.GetHashCode(), Transaction.GetHashCode(), Transaction.IsolationLevel));

                Transaction.Commit();

                _committed = true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);

                Rollback();
            }
            
        }

        public void Rollback()
        {
            if (!IsConnectionClosed())
            {
                if (!IsCommitted())
                {
                    try
                    {
                        _logger.Info(string.Format("Transaction rollback. ConnectionId:{0}, TransactionId:{1}, IsolationLevel:{2}", _parentRepositoryConnection.Connection.GetHashCode(), Transaction.GetHashCode(), Transaction.IsolationLevel));

                        Transaction.Rollback();

                        _committed = true;
                    }
                    catch (Exception ex)
                    {
                        _committed = true;

                        _logger.Error(ex);

                        throw;
                    }
                }
                else
                {
                    _logger.Info(string.Format("Transaction is already committed. ConnectionId:{0}", _parentRepositoryConnection.Connection.GetHashCode()));
                }
               
            }
            else
            {
                throw new Exception(string.Format("Connection is closed. ConnectionId:{0}", _parentRepositoryConnection.Connection.GetHashCode()));
            }
        }

        public void Dispose()
        {
            Rollback();

            _logger.Info(string.Format("Transaction disposed. ConnectionId:{0}", _parentRepositoryConnection.Connection.GetHashCode()));

            Transaction.Dispose();

            Transaction = null;

            _parentRepositoryConnection.CurrentRepositoryTransaction = null;

        }
    }
}