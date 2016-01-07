using System;

namespace Jal.Persistence.Fluent.Interface
{
    public interface IStoredProcedureDescriptor : IStoredProcedureInvoker
    {
        IStoredProcedureInvoker Parameters(Action<IParameterNameDescriptor> action);

        Action<IParameterNameDescriptor> ParameterAction { get; }

        string Name { get; }

    }
}