using System.Collections.Generic;
using Jal.Persistence.Fluent.Interface;
using Jal.Persistence.Model;

namespace Jal.Persistence.Fluent.Impl
{
    public class ParameterNameDescriptor : IParameterNameDescriptor
    {
        private readonly IList<Parameter> _parameters;

        public ParameterNameDescriptor(IList<Parameter> parameters)
        {
            _parameters = parameters;
        }

        public IParameterTypeDescriptor Name(string name)
        {
            var parameter = new Parameter { Name = name, Size = 0 };

            _parameters.Add(parameter);

            return new ParameterDescriptor(parameter);
        }
    }
}