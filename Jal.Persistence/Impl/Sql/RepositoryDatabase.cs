using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Jal.Persistence.Interface;
using Jal.Persistence.Model;

namespace Jal.Persistence.Impl.Sql
{
    public class RepositoryDatabase : IRepositoryDatabase
    {
        private readonly IRepositoryLogger _logger;

        private readonly IRepositorySettings _settings;

        public RepositoryDatabase(IRepositorySettings settings, IRepositoryLogger logger, IRepositoryCommand repositoryCommandInvoker)
        {
            _settings = settings;
            _logger = logger;
            RepositoryCommand = repositoryCommandInvoker;
        }

        public IRepositoryCommand RepositoryCommand { get; private set; }

        public IDbCommand CreateCommand(IDbConnection connection, string commandName)
        {
            var sqlCommand = connection.CreateCommand();
            sqlCommand.CommandText = commandName;
            sqlCommand.CommandTimeout = _settings.CommandTimeout;
            sqlCommand.CommandType = CommandType.StoredProcedure;
            return sqlCommand;
        }

        public IDbConnection CreateConnection()
        {
            if (string.IsNullOrEmpty(_settings.ConnectionString))
            {
                throw new InvalidOperationException("Cannot initialize dataaccess without a valid connection string.");
            }

            var connection = new SqlConnection(_settings.ConnectionString);

            if (_settings.StatisticsEnabled)
            {
                connection.StatisticsEnabled = true;
                connection.StateChange += StateChangeEventHandler;
            }

            _logger.Info(string.Format("Connection created. ConnectionId:{0}", connection.GetHashCode()));

            return connection;
        }

        public void StateChangeEventHandler(object sender, StateChangeEventArgs e)
        {
            if (e.CurrentState == ConnectionState.Closed)
            {
                var connection = sender as SqlConnection;
                if (connection.StatisticsEnabled)
                {
                    var d = connection.RetrieveStatistics() as Hashtable;
                    var s = string.Join(";", (from string name in d.Keys select name+":"+Convert.ToString(d[name])).ToArray());
                    _logger.Info(s);
                }
            }

        }

        public IDataParameter CreateParameter(string name, int size, ParameterType dbType, ParameterDirection direction, object value)
        {
            var sqlParameter = new SqlParameter()
            {
                ParameterName = CreateParameterName(name),
                Size = size,
                SqlDbType = Map(dbType),
                Value = value,
                Direction = direction

            };
            return sqlParameter;
        }

        public string CreateParameterName(string name)
        {
            return "@" + name;
        }

        private SqlDbType Map(ParameterType parameterType)
        {
            return (SqlDbType)parameterType;
        }
    }
}
