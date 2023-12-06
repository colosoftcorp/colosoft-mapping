namespace Colosoft.Mapping
{
    public interface IMappingConfigurationBuilderFactory
    {
        IMappingConfigurationBuilder<TTarget> Create<TTarget>();
    }
}
