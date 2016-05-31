using System;
using System.Data;
using System.Diagnostics;
using Jal.Persistence.Fluent.Impl;
using Jal.Persistence.Fluent.Interface;
using Jal.Persistence.Interface;

namespace Jal.Persistence.Impl
{
    public class RepositoryCommand : IRepositoryCommand
    {
        public static IRepositoryCommand Current;

        public static IRepositoryCommandStartFluentBuilder Builder
        {
            get
            {
                return new RepositoryCommandFluentBuilder();
            }
        }

        public RepositoryCommand()
        {
            Logger = AbstractRepositoryLogger.Instance;
        }

        public void Execute(Func<IDbConnection, string, IDbCommand> createCommand, IDbConnection connection, IDbTransaction transaction, string commandName, Action<IDbCommand> commandSetup)
        {
            try
            {
                var stopWatch = new Stopwatch();
                stopWatch.Start();
                IDataParameterCollection parameterCollection = null;
                try
                {
                    using (var cmd = createCommand(connection, commandName))
                    {
                        if (transaction != null)
                        {
                            cmd.Transaction = transaction;
                        }
                        if (commandSetup != null)
                        {
                            commandSetup(cmd);
                        }
                        parameterCollection = cmd.Parameters;
                        cmd.ExecuteNonQuery();
                    }
                }
                finally
                {
                    stopWatch.Stop();
                    Logger.Command(commandName, connection.Database, parameterCollection, stopWatch.Elapsed.TotalMilliseconds);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        public void Execute<TOutput>(Func<IDbConnection, string, IDbCommand> createCommand, IDbConnection connection, IDbTransaction transaction, string commandName, Action<TOutput, IDbCommand> outputReader, TOutput output, Action<IDbCommand> commandSetup = null) where TOutput : class
        {
            try
            {
                var stopWatch = new Stopwatch();
                stopWatch.Start();
                IDataParameterCollection parameterCollection = null;
                try
                {
                    using (var cmd = createCommand(connection, commandName))
                    {
                        if (transaction != null)
                        {
                            cmd.Transaction = transaction;
                        }
                        if (commandSetup != null)
                        {
                            commandSetup(cmd);
                        }
                        parameterCollection = cmd.Parameters;
                        cmd.ExecuteNonQuery();
                        if (outputReader != null && output != null)
                        {
                            outputReader(output, cmd);
                        }
                    }

                }
                finally
                {
                    stopWatch.Stop();
                    Logger.Command(commandName, connection.Database, parameterCollection, stopWatch.Elapsed.TotalMilliseconds);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }

        }

        public TData Query<TData>(Func<IDbConnection, string, IDbCommand> createCommand, IDbConnection connection, IDbTransaction transaction, string commandName, Func<IDataReader, TData> converter, Action<IDbCommand> commandSetup = null)
        {
            try
            {
                var stopWatch = new Stopwatch();
                stopWatch.Start();
                IDataParameterCollection parameterCollection = null;
                try
                {
                    using (var cmd = createCommand(connection, commandName))
                    {
                        if (transaction != null)
                        {
                            cmd.Transaction = transaction;
                        }
                        if (commandSetup != null)
                        {
                            commandSetup(cmd);
                        }
                        parameterCollection = cmd.Parameters;
                        TData data;
                        using (var dtr = cmd.ExecuteReader())
                        {
                            data = converter(dtr);
                        }
                        return data;
                    }
                }
                finally
                {
                    stopWatch.Stop();
                    Logger.Command(commandName, connection.Database, parameterCollection, stopWatch.Elapsed.TotalMilliseconds);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
            
        }

        public TData Query<TData, TOutput>(Func<IDbConnection, string, IDbCommand> createCommand, IDbConnection connection, IDbTransaction transaction, string commandName, Func<IDataReader, TData> converter, Action<TOutput, IDbCommand> outputReader, TOutput output, Action<IDbCommand> commandSetup = null)
            where TData : class
            where TOutput : class
        {
            try
            {
                var stopWatch = new Stopwatch();
                stopWatch.Start();
                IDataParameterCollection parameterCollection = null;
                try
                {
                    using (var cmd = createCommand(connection, commandName))
                    {
                        if (transaction != null)
                        {
                            cmd.Transaction = transaction;
                        }
                        if (commandSetup != null)
                        {
                            commandSetup(cmd);
                        }
                        parameterCollection = cmd.Parameters;

                        TData data;

                        using (var dtr = cmd.ExecuteReader())
                        {
                            data = converter(dtr);
                        }
                        if (outputReader != null && output != null)
                        {
                            outputReader(output, cmd);
                        }
                        return data;
                    }
                }
                finally
                {
                    stopWatch.Stop();
                    Logger.Command(commandName, connection.Database, parameterCollection, stopWatch.Elapsed.TotalMilliseconds);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
           
        }

        public object Scalar(Func<IDbConnection, string, IDbCommand> createCommand, IDbConnection connection, IDbTransaction transaction, string commandName, Action<IDbCommand> commandSetup)
        {
            try
            {
                var stopWatch = new Stopwatch();
                stopWatch.Start();
                IDataParameterCollection parameterCollection = null;
                try
                {
                    using (var cmd = createCommand(connection, commandName))
                    {
                        if (transaction != null)
                        {
                            cmd.Transaction = transaction;
                        }
                        if (commandSetup != null)
                        {
                            commandSetup(cmd);
                        }
                        parameterCollection = cmd.Parameters;
                        return cmd.ExecuteScalar();
                    }
                }
                finally
                {
                    stopWatch.Stop();
                    Logger.Command(commandName, connection.Database, parameterCollection, stopWatch.Elapsed.TotalMilliseconds);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        public IRepositoryLogger Logger { get; set; }
    }
}