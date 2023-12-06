namespace Colosoft.Mapping
{
    public interface IMappingValidatorFactory
    {
        bool IsCompatibillity(IMappingDataSourceSchema source);

        IMappingValidator Create(IMappingDataSourceSchema source);
    }
}
