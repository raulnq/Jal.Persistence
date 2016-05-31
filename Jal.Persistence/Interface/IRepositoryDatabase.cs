using System.Data;
using Jal.Persistence.Model;

namespace Jal.Persistence.Interface
{
    public interface IRepositoryDatabase
    {
        IDbCommand CreateCommand(IDbConnection connection, string name);

        IDbConnection CreateConnection();

        IDataParameter CreateParameter(string name, int size, ParameterType type, ParameterDirection direction, object value);

        string CreateParameterName(string name);

        IRepositoryLogger Logger { get;}
    }
}
