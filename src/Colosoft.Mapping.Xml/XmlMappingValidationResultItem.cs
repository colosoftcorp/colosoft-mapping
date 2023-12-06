namespace Colosoft.Mapping.Xml
{
    public class XmlMappingValidationResultItem : IMappingValidationResultItem
    {
        public XmlMappingValidationResultItem(string description)
        {
            this.Description = description;
        }

        public string Description { get; }
    }
}
