using System;
using System.Data;
using System.Data.SqlTypes;
using System.Linq.Expressions;
using Jal.Persistence.Fluent.Interface;
using Jal.Persistence.Model;

namespace Jal.Persistence.Fluent.Impl
{
    public class ParameterDescriptor : IParameterTypeDescriptor, IParameterDirectionDescriptor, IParameterReaderDescriptor, IParameterValueDescriptor
    {
        private readonly Parameter _parameter;

        public ParameterDescriptor(Parameter parameter)
        {
            _parameter = parameter;
        }

        public void Value(object value)
        {
            if (value == null)
            {
                _parameter.Value = DBNull.Value;
                return;
            }

            if (_parameter.Type == ParameterType.VarChar || _parameter.Type == ParameterType.Char || _parameter.Type == ParameterType.Text)
            {
                _parameter.Value = new SqlString((string)value);
            }

            if (_parameter.Type == ParameterType.Int)
            {
                _parameter.Value = new SqlInt32(Convert.ToInt32(value));
            }

            if (_parameter.Type == ParameterType.TinyInt)
            {
                _parameter.Value = new SqlByte(Convert.ToByte(value));
            }

            if (_parameter.Type == ParameterType.Bit)
            {
                _parameter.Value = new SqlBoolean((bool)value);
            }

            if (_parameter.Type == ParameterType.BigInt)
            {
                _parameter.Value = new SqlInt64(Convert.ToInt64(value));
            }

            if (_parameter.Type == ParameterType.DateTime || _parameter.Type == ParameterType.SmallDateTime)
            {
                _parameter.Value = new SqlDateTime(Convert.ToDateTime(value));
            }

            if (_parameter.Type == ParameterType.SmallInt)
            {
                _parameter.Value = new SqlInt16(Convert.ToInt16(value));
            }

            if (_parameter.Type == ParameterType.Decimal)
            {
                _parameter.Value = new SqlDecimal(Convert.ToDecimal(value));
            }

            if (_parameter.Type == ParameterType.Money)
            {
                _parameter.Value = new SqlMoney(Convert.ToDecimal(value));
            }

            if (_parameter.Type == ParameterType.UniqueIdentifier)
            {
                _parameter.Value = new SqlGuid((Guid)value);
            }
        }

        public void To<TSource, TProperty>(Expression<Func<TSource, TProperty>> action)
        {
            _parameter.Reader = action;
        }

        public IParameterReaderDescriptor Out()
        {
            _parameter.Direction = ParameterDirection.Output;
            return this;
        }

        public IParameterReaderDescriptor Return()
        {
            _parameter.Direction = ParameterDirection.ReturnValue;
            return this;
        }

        public IParameterValueDescriptor In()
        {
            _parameter.Direction = ParameterDirection.Input;
            return this;
        }

        public IParameterDirectionDescriptor VarChar(int size)
        {
            _parameter.Type = ParameterType.VarChar;
            _parameter.Size = size;
            return this;
        }

        public IParameterDirectionDescriptor Int()
        {
            _parameter.Type = ParameterType.Int;
            return this;
        }

        public IParameterDirectionDescriptor TinyInt()
        {
            _parameter.Type = ParameterType.TinyInt;
            return this;
        }

        public IParameterDirectionDescriptor Bit()
        {
            _parameter.Type = ParameterType.Bit;
            return this;
        }

        public IParameterDirectionDescriptor BigInt()
        {
            _parameter.Type = ParameterType.BigInt;
            return this;
        }

        public IParameterDirectionDescriptor DateTime()
        {
            _parameter.Type = ParameterType.DateTime;
            return this;
        }

        public IParameterDirectionDescriptor SmallDateTime()
        {
            _parameter.Type = ParameterType.SmallDateTime;
            return this;
        }

        public IParameterDirectionDescriptor SmallInt()
        {
            _parameter.Type = ParameterType.SmallInt;
            return this;
        }

        public IParameterDirectionDescriptor Decimal()
        {
            _parameter.Type = ParameterType.Decimal;
            return this;
        }

        public IParameterDirectionDescriptor Char()
        {
            _parameter.Type = ParameterType.Char;
            return this;
        }

        public IParameterDirectionDescriptor Money()
        {
            _parameter.Type = ParameterType.Money;
            return this;
        }

        public IParameterDirectionDescriptor Guid()
        {
            _parameter.Type = ParameterType.UniqueIdentifier;
            return this;
        }

        public IParameterDirectionDescriptor Xml()
        {
            _parameter.Type = ParameterType.Xml;
            return this;
        }

        public IParameterDirectionDescriptor Text(int size)
        {
            _parameter.Type = ParameterType.Text;
            _parameter.Size = size;
            return this;
        }
    }
}