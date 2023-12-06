namespace Colosoft.Mapping
{
    public class MappingConfigurationBuilderFactory : IMappingConfigurationBuilderFactory
    {
        public IMappingConfigurationBuilder<TTarget> Create<TTarget>()
        {
            return new MappingConfigurationBuilder<TTarget>();
        }
    }
}
