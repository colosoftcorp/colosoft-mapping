using System.Xml;

namespace Colosoft.Mapping.Xml
{
    internal class XmlMappingDataSourceRecord : IMappingDataSourceRecord
    {
        private readonly XmlNode node;

        public XmlMappingDataSourceRecord(XmlNode node, IMappingDataSourceSchema schema)
        {
            this.node = node;
            this.Schema = schema;
        }

        public IMappingDataSourceSchema Schema { get; }

        public object GetValue(string fieldName)
        {
            return this.node.SelectSingleNode(fieldName).Value;
        }
    }
}