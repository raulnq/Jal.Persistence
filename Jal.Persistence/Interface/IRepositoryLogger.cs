using System;
using System.Data;

namespace Jal.Persistence.Interface
{
    public interface IRepositoryLogger
    {
        void Error(Exception e);

        void Info(string message);

        void Command(string commandName, string databaseName, IDataParameterCollection parameters, double duration);
    }
}
