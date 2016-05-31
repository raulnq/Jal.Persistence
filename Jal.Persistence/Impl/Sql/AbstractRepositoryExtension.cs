using System;
using System.Data;

namespace Jal.Persistence.Impl.Sql
{
    public static class AbstractRepositoryExtension
    {
        public static string QueryXml(this AbstractRepository repository, string commandName, Action<IDbCommand> commandSetup = null)
        {
            if (repository.Context.CurrentConnection != null)
            {
                if (repository.Context.CurrentConnection.CurrentTransaction != null)
                {

                    return repository.Command.QueryXml(repository.Context.Database.CreateCommand, repository.Context.CurrentConnection.Connection, repository.Context.CurrentConnection.CurrentTransaction.Transaction, commandName, commandSetup);
                }
                else
                {
                    return repository.Command.QueryXml(repository.Context.Database.CreateCommand, repository.Context.CurrentConnection.Connection, null, commandName, commandSetup);
                }

            }
            else
            {
                using (var connection = repository.Context.Database.CreateConnection())
                {
                    connection.Open();
                    return repository.Command.QueryXml(repository.Context.Database.CreateCommand, connection, null, commandName, commandSetup);
                }
            }
        }

    }
}
