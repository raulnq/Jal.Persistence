using System;
using System.Data;

namespace Jal.Persistence.Impl.Sql
{
    public static class AbstractRepositoryExtension
    {
        public static string QueryXml(this AbstractRepository repository, string commandName, Action<IDbCommand> commandSetup = null)
        {
            if (repository.Context.CurrentRepositoryConnection != null)
            {
                if (repository.Context.CurrentRepositoryConnection.CurrentRepositoryTransaction != null)
                {

                    return repository.Context.Command.QueryXml(repository.Context.Database.CreateCommand, repository.Context.CurrentRepositoryConnection.Connection, repository.Context.CurrentRepositoryConnection.CurrentRepositoryTransaction.Transaction, commandName, commandSetup);
                }
                else
                {
                    return repository.Context.Command.QueryXml(repository.Context.Database.CreateCommand, repository.Context.CurrentRepositoryConnection.Connection, null, commandName, commandSetup);
                }

            }
            else
            {
                using (var connection = repository.Context.Database.CreateConnection())
                {
                    connection.Open();
                    return repository.Context.Command.QueryXml(repository.Context.Database.CreateCommand, connection, null, commandName, commandSetup);
                }
            }
        }

    }
}
