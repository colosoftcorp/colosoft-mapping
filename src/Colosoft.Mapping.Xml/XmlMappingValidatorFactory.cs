namespace Colosoft.Mapping.Xml
{
    public class XmlMappingValidatorFactory : IMappingValidatorFactory
    {
        public bool IsCompatibillity(IMappingDataSourceSchema source) => true;

        public IMappingValidator Create(IMappingDataSourceSchema source)
        {
            return new XmlMappingValidator(source);
        }
    }
}
