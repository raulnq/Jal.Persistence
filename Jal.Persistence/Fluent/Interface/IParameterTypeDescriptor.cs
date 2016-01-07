namespace Jal.Persistence.Fluent.Interface
{
    public interface IParameterTypeDescriptor
    {
        IParameterDirectionDescriptor VarChar(int size);

        IParameterDirectionDescriptor Int();

        IParameterDirectionDescriptor TinyInt();

        IParameterDirectionDescriptor Bit();

        IParameterDirectionDescriptor BigInt();

        IParameterDirectionDescriptor DateTime();

        IParameterDirectionDescriptor SmallDateTime();

        IParameterDirectionDescriptor SmallInt();

        IParameterDirectionDescriptor Decimal();

        IParameterDirectionDescriptor Char();

        IParameterDirectionDescriptor Money();

        IParameterDirectionDescriptor Guid();

        IParameterDirectionDescriptor Xml();

        IParameterDirectionDescriptor Text(int size);
    }
}