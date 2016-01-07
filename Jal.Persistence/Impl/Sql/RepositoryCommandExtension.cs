using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using Jal.Persistence.Interface;

namespace Jal.Persistence.Impl.Sql
{
    public static class RepositoryCommandExtension
    {
        public static string QueryXml(this IRepositoryCommand repositoryCommand, Func<IDbConnection, string, IDbCommand> createCommand, IDbConnection connection, IDbTransaction transaction, string commandName, Action<IDbCommand> commandSetup)
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
                        var sqlcmd = cmd as SqlCommand;
                        if (sqlcmd != null)
                        {
                            var xmlReader = sqlcmd.ExecuteXmlReader();
                            xmlReader.MoveToContent();
                            return xmlReader.ReadOuterXml();
                        }
                        else
                        {
                            throw new Exception("Unsupported ExecuteXmlReader");
                        }
                    }
                }
                finally
                {
                    stopWatch.Stop();
                    repositoryCommand.RepositoryLogger.Command(commandName, connection.Database, parameterCollection, stopWatch.Elapsed.TotalMilliseconds);
                }
            }
            catch (Exception ex)
            {
                repositoryCommand.RepositoryLogger.Error(ex);
                throw;
            }
        }
    }
}
