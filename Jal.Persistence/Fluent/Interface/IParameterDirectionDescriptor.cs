namespace Jal.Persistence.Fluent.Interface
{
    public interface IParameterDirectionDescriptor
    {
        IParameterReaderDescriptor Out();

        IParameterReaderDescriptor Return();

        IParameterValueDescriptor In();
    }
}