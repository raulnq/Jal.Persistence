using System;
using System.Data;
using Jal.Converter.Interface;
using Jal.Persistence.Fluent.Impl;
using Jal.Persistence.Fluent.Interface;
using Jal.Persistence.Interface;

namespace Jal.Persistence.Impl
{
    public abstract class AbstractRepository
    {
        protected IModelConverter ModelConverter;

        private readonly IRepositoryContext _repositoryContext;

        protected AbstractRepository(IRepositoryContext repositoryContext, IModelConverter modelConverter)
        {
            ModelConverter = modelConverter;
            _repositoryContext = repositoryContext;
        }

        public TData Query<TData>(string commandName, Action<IDbCommand> commandSetup = null) where TData : class
        {
            if (_repositoryContext.CurrentRepositoryConnection != null)
            {
                if (_repositoryContext.CurrentRepositoryConnection.CurrentRepositoryTransaction != null)
                {

                    return _repositoryContext.Command.Query(_repositoryContext.Database.CreateCommand, _repositoryContext.CurrentRepositoryConnection.Connection, _repositoryContext.CurrentRepositoryConnection.CurrentRepositoryTransaction.Transaction, commandName, x => ModelConverter.Convert<IDataReader, TData>(x), commandSetup);
                }
                else
                {
                    return _repositoryContext.Command.Query(_repositoryContext.Database.CreateCommand, _repositoryContext.CurrentRepositoryConnection.Connection, null, commandName, x => ModelConverter.Convert<IDataReader, TData>(x), commandSetup);
                }

            }
            else
            {
                using (var connection = _repositoryContext.Database.CreateConnection())
                {
                    connection.Open();
                    return _repositoryContext.Command.Query(_repositoryContext.Database.CreateCommand, connection, null, commandName, x => ModelConverter.Convert<IDataReader, TData>(x), commandSetup);
                }
            }
        }

        public object Scalar(string commandName, Action<IDbCommand> commandSetup = null)
        {
            if (_repositoryContext.CurrentRepositoryConnection != null)
            {
                if (_repositoryContext.CurrentRepositoryConnection.CurrentRepositoryTransaction != null)
                {

                    return _repositoryContext.Command.Scalar(_repositoryContext.Database.CreateCommand, _repositoryContext.CurrentRepositoryConnection.Connection, _repositoryContext.CurrentRepositoryConnection.CurrentRepositoryTransaction.Transaction, commandName, commandSetup);
                }
                else
                {
                    return _repositoryContext.Command.Scalar(_repositoryContext.Database.CreateCommand, _repositoryContext.CurrentRepositoryConnection.Connection, null, commandName, commandSetup);
                }

            }
            else
            {
                using (var connection = _repositoryContext.Database.CreateConnection())
                {
                    connection.Open();
                    return _repositoryContext.Command.Scalar(_repositoryContext.Database.CreateCommand, connection, null, commandName, commandSetup);
                }
            }
        }

        public TData Query<TData, TOutput>(string commandName, Action<TOutput, IDbCommand> outputReader, TOutput output, Action<IDbCommand> commandSetup = null) where TData : class where TOutput : class
        {
            if (_repositoryContext.CurrentRepositoryConnection != null)
                {
                    if (_repositoryContext.CurrentRepositoryConnection.CurrentRepositoryTransaction != null)
                    {
                        return _repositoryContext.Command.Query(_repositoryContext.Database.CreateCommand, _repositoryContext.CurrentRepositoryConnection.Connection, _repositoryContext.CurrentRepositoryConnection.CurrentRepositoryTransaction.Transaction, commandName, x => ModelConverter.Convert<IDataReader, TData>(x), outputReader, output, commandSetup);
                    }
                    else
                    {
                        return _repositoryContext.Command.Query(_repositoryContext.Database.CreateCommand, _repositoryContext.CurrentRepositoryConnection.Connection, null, commandName, x => ModelConverter.Convert<IDataReader, TData>(x), outputReader, output, commandSetup);
                    }
                }
                else
                {
                    using (var connection = _repositoryContext.Database.CreateConnection())
                    {
                        connection.Open();
                        return _repositoryContext.Command.Query(_repositoryContext.Database.CreateCommand, connection, null, commandName, x => ModelConverter.Convert<IDataReader, TData>(x), outputReader, output, commandSetup);
                    }
                }
        }

        public void Execute<TOutput>(string commandName, Action<TOutput, IDbCommand> outputReader, TOutput output, Action<IDbCommand> commandSetup = null) where TOutput : class
        {
            if (_repositoryContext.CurrentRepositoryConnection != null)
            {
                if (_repositoryContext.CurrentRepositoryConnection.CurrentRepositoryTransaction != null)
                {
                    _repositoryContext.Command.Execute(_repositoryContext.Database.CreateCommand, _repositoryContext.CurrentRepositoryConnection.Connection, _repositoryContext.CurrentRepositoryConnection.CurrentRepositoryTransaction.Transaction, commandName, outputReader, output, commandSetup);
                }
                else
                {
                    _repositoryContext.Command.Execute(_repositoryContext.Database.CreateCommand, _repositoryContext.CurrentRepositoryConnection.Connection, null, commandName, outputReader, output, commandSetup);
                }
            }
            else
            {
                using (var connection = _repositoryContext.Database.CreateConnection())
                {
                    connection.Open();
                    _repositoryContext.Command.Execute(_repositoryContext.Database.CreateCommand, connection, null, commandName, outputReader, output, commandSetup);
                }
            }
        }

        public void Execute(string commandName, Action<IDbCommand> commandSetup = null)
        {
            if (_repositoryContext.CurrentRepositoryConnection != null)
            {
                if (_repositoryContext.CurrentRepositoryConnection.CurrentRepositoryTransaction != null)
                {
                    _repositoryContext.Command.Execute(_repositoryContext.Database.CreateCommand, _repositoryContext.CurrentRepositoryConnection.Connection, _repositoryContext.CurrentRepositoryConnection.CurrentRepositoryTransaction.Transaction, commandName, commandSetup);
                }
                else
                {
                    _repositoryContext.Command.Execute(_repositoryContext.Database.CreateCommand, _repositoryContext.CurrentRepositoryConnection.Connection, null, commandName, commandSetup);
                }
            }
            else
            {
                using (var connection = _repositoryContext.Database.CreateConnection())
                {
                    connection.Open();
                    _repositoryContext.Command.Execute(_repositoryContext.Database.CreateCommand, connection, null, commandName, commandSetup);
                }
            }
        }

        public IRepositoryContext Context 
        {
            get { return _repositoryContext; }
        }

        public IStoredProcedureDescriptor StoredProcedure(string name)
        {
            return new StoredProcedureDescriptor(this, name);
        }
    }
}