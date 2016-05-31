using System;
using System.Data;
using Jal.Persistence.Interface;

namespace Jal.Persistence.Impl
{
    public abstract class AbstractRepositoryLogger: IRepositoryLogger
    {
        public static IRepositoryLogger Instance = new NullRepositoryLogger();

        public virtual void Error(Exception e)
        {
            
        }

        public virtual void Info(string message)
        {
            
        }

        public virtual void Command(string commandName, string databaseName, IDataParameterCollection parameters, double duration)
        {
            
        }
    }
}