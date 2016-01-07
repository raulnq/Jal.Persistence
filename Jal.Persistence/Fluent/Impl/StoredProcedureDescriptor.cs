using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Jal.Persistence.Fluent.Interface;
using Jal.Persistence.Impl;
using Jal.Persistence.Model;

namespace Jal.Persistence.Fluent.Impl
{
    public class StoredProcedureDescriptor : IStoredProcedureDescriptor
    {
        private readonly string _name;

        private readonly AbstractRepository _repository;

        private Action<IParameterNameDescriptor> _parameterAction;

        public StoredProcedureDescriptor(AbstractRepository repository, string name)
        {
            _repository = repository;
            _name = name;
            _parameterAction = x => { };
        }

        public IStoredProcedureInvoker Parameters(Action<IParameterNameDescriptor> action)
        {
            _parameterAction = action;

            return this;
        }

        public AbstractRepository AbstractRepository {
            get
            {
                return _repository; 
            }
        }

        public Action<IParameterNameDescriptor> ParameterAction
        {
            get
            {
                return _parameterAction; 
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
        }

        public void Execute()
        {
            var parameters = new List<Parameter>();

            var parameterDescriptor = new ParameterNameDescriptor(parameters);

            _parameterAction(parameterDescriptor);

            var commandSetup = CreateCommandSetup(parameters);

            _repository.Execute(_name, commandSetup);
        }

        public object Scalar()
        {
            var parameters = new List<Parameter>();

            var parameterDescriptor = new ParameterNameDescriptor(parameters);

            _parameterAction(parameterDescriptor);

            var commandSetup = CreateCommandSetup(parameters);

            return _repository.Scalar(_name, commandSetup);
        }

        public void Execute<TOutput>(TOutput output) where TOutput : class
        {
            var parameters = new List<Parameter>();

            var parameterDescriptor = new ParameterNameDescriptor(parameters);

            _parameterAction(parameterDescriptor);

            var commandSetup = CreateCommandSetup(parameters);

            var outputReader = CreateOutputReader(output, parameters);

            _repository.Execute(_name, outputReader, output, commandSetup);
        }

        public TData Query<TData>() where TData : class
        {
            var parameters = new List<Parameter>();

            var parameterDescriptor = new ParameterNameDescriptor(parameters);

            _parameterAction(parameterDescriptor);

            var commandSetup = CreateCommandSetup(parameters);

            return _repository.Query<TData>(_name, commandSetup);
        }

        public TData Query<TData, TOutput>(TOutput output)
            where TData : class
            where TOutput : class
        {
            var parameters = new List<Parameter>();

            var parameterDescriptor = new ParameterNameDescriptor(parameters);

            _parameterAction(parameterDescriptor);

            var commandSetup = CreateCommandSetup(parameters);

            var outputReader = CreateOutputReader(output, parameters);

            return _repository.Query<TData, TOutput>(_name, outputReader, output, commandSetup);
        }

        

        private Action<TOutput, IDbCommand> CreateOutputReader<TOutput>(TOutput output, List<Parameter> parameters) where TOutput : class
        {
            Action<TOutput, IDbCommand> readerAction = (o, x) =>
            {
                var sqlcommand = x as DbCommand;
                foreach (var parameter in parameters.Where(p => p.Reader != null))
                {
                    var value = sqlcommand.Parameters[_repository.Context.Database.CreateParameterName(parameter.Name)].Value;
                    var memberSelectorExpression =
                        parameter.Reader.Body as MemberExpression;
                    if (memberSelectorExpression != null)
                    {
                        var property = memberSelectorExpression.Member as PropertyInfo;
                        if (property != null)
                        {
                            if (value is DBNull)
                            {
                                property.SetValue(output, null, null);
                            }
                            else
                            {
                                property.SetValue(output, value, null);
                            }
                        }
                    }
                }
            };
            return readerAction;
        }

        private Action<IDbCommand> CreateCommandSetup(List<Parameter> parameters)
        {
            Action<IDbCommand> inputAction = x =>
            {
                foreach (var parameter in parameters)
                {
                    var sqlParameter = _repository.Context.Database.CreateParameter(parameter.Name, parameter.Size, parameter.Type, parameter.Direction, parameter.Value);
                    x.Parameters.Add(sqlParameter);
                }
            };
            return inputAction;
        }
    }
}