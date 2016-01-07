using System.Data;
using System.Linq.Expressions;

namespace Jal.Persistence.Model
{
    public class Parameter
    {
        public ParameterDirection Direction { get; set; }

        public string Name { get; set; }

        public LambdaExpression Reader { get; set; }

        public int Size { get; set; }

        public ParameterType Type { get; set; }

        public object Value { get; set; }
    }
}