namespace Colosoft.Mapping.Xml
{
    internal class XmlMappingDataSourcePreparer : IMappingDataSourcePreparer
    {
        public XmlMappingDataSourcePreparer(string name)
        {
            this.Name = name;
        }

        public string Name { get; }
    }
}
