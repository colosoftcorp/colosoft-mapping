using System.Collections.Generic;

namespace Colosoft.Mapping.Xml
{
    internal class XmlMappingDataSourceSchema : IMappingDataSourceSchema
    {
        public XmlMappingDataSourceSchema(IEnumerable<IMappingDataSourceSchemaField> fields)
        {
            this.Fields = fields;
        }

        public IEnumerable<IMappingDataSourceSchemaField> Fields { get; }
    }
}