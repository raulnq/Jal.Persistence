using System;
using System.Linq.Expressions;

namespace Jal.Persistence.Fluent.Interface
{
    public interface IParameterReaderDescriptor
    {
        void To<TSource, TProperty>(Expression<Func<TSource, TProperty>> action);
    }
}