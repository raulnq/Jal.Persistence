using System;
using System.Data;
using Jal.Persistence.Interface;

namespace Jal.Persistence.Impl
{
    public class NullRepositoryLogger: IRepositoryLogger
    {

        public void Error(Exception e)
        {
            
        }

        public void Info(string message)
        {
            
        }

        public void Command(string commandName, string databaseName, IDataParameterCollection parameters, double duration)
        {
            
        }
    }
}
