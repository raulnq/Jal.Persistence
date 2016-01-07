using Jal.Persistence.Impl;

namespace Jal.Persistence.Fluent.Interface
{
    public interface IStoredProcedureInvoker
    {
        void Execute();

        object Scalar();

        void Execute<TOutput>(TOutput output) where TOutput : class;

        TData Query<TData>() where TData : class;

        TData Query<TData, TOutput>(TOutput output)
            where TData : class
            where TOutput : class;

        AbstractRepository AbstractRepository { get; }
    }
}