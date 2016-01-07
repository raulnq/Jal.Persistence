using System;
using System.Data;
using System.Data.Common;
using Common.Logging;
using Jal.Persistence.Interface;

namespace Jal.Persistence.Logger
{
    public class RepositoryLogger: IRepositoryLogger
    {
        private readonly ILog _log;

        public RepositoryLogger(ILog log)
        {
            _log = log;
        }

        public void Error(Exception e)
        {
            _log.Error(e);
        }

        public void Info(string message)
        {
            _log.Info(message);
        }

        public void Command(string commandText, string databaseName, IDataParameterCollection parameters, double duration)
        {
            var sqlScript = GetSqlScript(commandText, databaseName, (DbParameterCollection)parameters);
            var message = string.Format("DB {0} took: {1} milliseconds.", sqlScript, duration);
            _log.Debug(message);
        }

        private string GetSqlScript(string commandText, string database, DbParameterCollection parameters)
        {
            var sb = new System.Text.StringBuilder();

            sb.Append(string.Format("EXEC {0}.{1} ", database, commandText));

            if (parameters!=null && parameters.Count > 0 )
            {
                for (var i = 0; i <= parameters.Count - 1; i++)
                {
                    switch (parameters[i].DbType)
                    {
                        case DbType.AnsiString:
                        case DbType.Date:
                        case DbType.DateTime:
                        case DbType.String:
                        case DbType.Time:
                        case DbType.AnsiStringFixedLength:
                        case DbType.StringFixedLength:
                            {
                                if (parameters[i].Value != null)
                                {
                                    if (parameters[i].Value.ToString().ToLower() == "null")
                                        sb.Append(string.Format("{0} = NULL, ", parameters[i].ParameterName));
                                    else
                                        sb.Append(string.Format("{0} = '{1}', ", parameters[i].ParameterName,
                                                                parameters[i].Value));
                                }
                                else
                                {
                                    sb.Append(string.Format("{0} = NULL, ", parameters[i].ParameterName));
                                }
                                break;
                            }
                        case DbType.Boolean:
                            {
                                switch (parameters[i].Value.ToString().ToLower())
                                {
                                    case "null":
                                        sb.Append(string.Format("{0} = NULL, ", parameters[i].ParameterName));
                                        break;
                                    case "true":
                                        sb.Append(string.Format("{0} = 1, ", parameters[i].ParameterName));
                                        break;
                                    default:
                                        sb.Append(string.Format("{0} = 0, ", parameters[i].ParameterName));
                                        break;
                                }
                                break;
                            }

                        default:
                            if (parameters[i].Value != null)
                            {
                                if (parameters[i].Value.ToString().ToLower() == "null")
                                    sb.Append(string.Format("{0} = NULL, ", parameters[i].ParameterName));
                                else
                                    sb.Append(string.Format("{0} = {1}, ", parameters[i].ParameterName, parameters[i].Value));
                            }
                            else
                            {
                                sb.Append(string.Format("{0} = NULL, ", parameters[i].ParameterName));
                            }

                            break;
                    }
                }
            }
            return sb.ToString().Trim().TrimEnd(',').Trim();
        }


    }
}