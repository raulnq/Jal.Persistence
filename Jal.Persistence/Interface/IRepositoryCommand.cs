using System;
using System.Data;

namespace Jal.Persistence.Interface
{
    public interface IRepositoryCommand
    {
        void Execute(Func<IDbConnection,string,IDbCommand> createCommand, IDbConnection connection, IDbTransaction transaction, string commandName, Action<IDbCommand> commandSetup = null);

        void Execute<TOutput>(Func<IDbConnection, string, IDbCommand> createCommand, IDbConnection connection, IDbTransaction transaction, string commandName, Action<TOutput, IDbCommand> outputReader, TOutput output, Action<IDbCommand> commandSetup = null) where TOutput : class;

        TData Query<TData>(Func<IDbConnection, string, IDbCommand> createCommand, IDbConnection connection, IDbTransaction transaction, string commandName, Func<IDataReader, TData> converter, Action<IDbCommand> commandSetup = null);

        TData Query<TData, TOutput>(Func<IDbConnection, string, IDbCommand> createCommand, IDbConnection connection, IDbTransaction transaction, string commandName, Func<IDataReader, TData> converter, Action<TOutput, IDbCommand> outputReader, TOutput output, Action<IDbCommand> commandSetup = null) where TData : class where TOutput : class;

        object Scalar(Func<IDbConnection, string, IDbCommand> createCommand, IDbConnection connection, IDbTransaction transaction, string commandName, Action<IDbCommand> commandSetup);

        IRepositoryLogger RepositoryLogger { get; }
    }
}