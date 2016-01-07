using System;
using System.Collections.Generic;
using System.Data;
using Jal.Persistence.Fluent.Interface;
using Jal.Persistence.Impl.Sql;
using Jal.Persistence.Model;

namespace Jal.Persistence.Fluent.Impl.Sql
{
    public static class StoredProcedureDescriptorExtension
    {
        public static string QueryXml(this IStoredProcedureDescriptor storedProcedureDescriptor)
        {
            var parameters = new List<Parameter>();

            var parameterDescriptor = new ParameterNameDescriptor(parameters);

            storedProcedureDescriptor.ParameterAction(parameterDescriptor);

            Action<IDbCommand> inputAction = x =>
            {
                foreach (var parameter in parameters)
                {
                    var sqlParameter = storedProcedureDescriptor.AbstractRepository.Context.Database.CreateParameter(parameter.Name, parameter.Size, parameter.Type, parameter.Direction, parameter.Value);
                    x.Parameters.Add(sqlParameter);
                }
            };

            return storedProcedureDescriptor.AbstractRepository.QueryXml(storedProcedureDescriptor.Name, inputAction);
        }
    }
}
