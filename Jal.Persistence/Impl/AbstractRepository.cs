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

        public IRepositoryCommand Command { get; set; } 

        protected AbstractRepository(IRepositoryContext repositoryContext, IModelConverter modelConverter, IRepositoryCommand repositoryCommand)
        {
            ModelConverter = modelConverter;

            Context = repositoryContext;

            Command = repositoryCommand;
        }

        public TData Query<TData>(string commandName, Action<IDbCommand> commandSetup = null) where TData : class
        {
            if (Context.CurrentConnection != null)
            {
                if (Context.CurrentConnection.CurrentTransaction != null)
                {

                    return Command.Query(Context.Database.CreateCommand, Context.CurrentConnection.Connection, Context.CurrentConnection.CurrentTransaction.Transaction, commandName, x => ModelConverter.Convert<IDataReader, TData>(x), commandSetup);
                }
                else
                {
                    return Command.Query(Context.Database.CreateCommand, Context.CurrentConnection.Connection, null, commandName, x => ModelConverter.Convert<IDataReader, TData>(x), commandSetup);
                }

            }
            else
            {
                using (var connection = Context.Database.CreateConnection())
                {
                    connection.Open();
                    return Command.Query(Context.Database.CreateCommand, connection, null, commandName, x => ModelConverter.Convert<IDataReader, TData>(x), commandSetup);
                }
            }
        }

        public object Scalar(string commandName, Action<IDbCommand> commandSetup = null)
        {
            if (Context.CurrentConnection != null)
            {
                if (Context.CurrentConnection.CurrentTransaction != null)
                {

                    return Command.Scalar(Context.Database.CreateCommand, Context.CurrentConnection.Connection, Context.CurrentConnection.CurrentTransaction.Transaction, commandName, commandSetup);
                }
                else
                {
                    return Command.Scalar(Context.Database.CreateCommand, Context.CurrentConnection.Connection, null, commandName, commandSetup);
                }

            }
            else
            {
                using (var connection = Context.Database.CreateConnection())
                {
                    connection.Open();
                    return Command.Scalar(Context.Database.CreateCommand, connection, null, commandName, commandSetup);
                }
            }
        }

        public TData Query<TData, TOutput>(string commandName, Action<TOutput, IDbCommand> outputReader, TOutput output, Action<IDbCommand> commandSetup = null) where TData : class where TOutput : class
        {
            if (Context.CurrentConnection != null)
                {
                    if (Context.CurrentConnection.CurrentTransaction != null)
                    {
                        return Command.Query(Context.Database.CreateCommand, Context.CurrentConnection.Connection, Context.CurrentConnection.CurrentTransaction.Transaction, commandName, x => ModelConverter.Convert<IDataReader, TData>(x), outputReader, output, commandSetup);
                    }
                    else
                    {
                        return Command.Query(Context.Database.CreateCommand, Context.CurrentConnection.Connection, null, commandName, x => ModelConverter.Convert<IDataReader, TData>(x), outputReader, output, commandSetup);
                    }
                }
                else
                {
                    using (var connection = Context.Database.CreateConnection())
                    {
                        connection.Open();
                        return Command.Query(Context.Database.CreateCommand, connection, null, commandName, x => ModelConverter.Convert<IDataReader, TData>(x), outputReader, output, commandSetup);
                    }
                }
        }

        public void Execute<TOutput>(string commandName, Action<TOutput, IDbCommand> outputReader, TOutput output, Action<IDbCommand> commandSetup = null) where TOutput : class
        {
            if (Context.CurrentConnection != null)
            {
                if (Context.CurrentConnection.CurrentTransaction != null)
                {
                    Command.Execute(Context.Database.CreateCommand, Context.CurrentConnection.Connection, Context.CurrentConnection.CurrentTransaction.Transaction, commandName, outputReader, output, commandSetup);
                }
                else
                {
                    Command.Execute(Context.Database.CreateCommand, Context.CurrentConnection.Connection, null, commandName, outputReader, output, commandSetup);
                }
            }
            else
            {
                using (var connection = Context.Database.CreateConnection())
                {
                    connection.Open();
                    Command.Execute(Context.Database.CreateCommand, connection, null, commandName, outputReader, output, commandSetup);
                }
            }
        }

        public void Execute(string commandName, Action<IDbCommand> commandSetup = null)
        {
            if (Context.CurrentConnection != null)
            {
                if (Context.CurrentConnection.CurrentTransaction != null)
                {
                    Command.Execute(Context.Database.CreateCommand, Context.CurrentConnection.Connection, Context.CurrentConnection.CurrentTransaction.Transaction, commandName, commandSetup);
                }
                else
                {
                    Command.Execute(Context.Database.CreateCommand, Context.CurrentConnection.Connection, null, commandName, commandSetup);
                }
            }
            else
            {
                using (var connection = Context.Database.CreateConnection())
                {
                    connection.Open();
                    Command.Execute(Context.Database.CreateCommand, connection, null, commandName, commandSetup);
                }
            }
        }

        public IRepositoryContext Context { get; private set; }

        public IStoredProcedureDescriptor StoredProcedure(string name)
        {
            return new StoredProcedureDescriptor(this, name);
        }
    }
}