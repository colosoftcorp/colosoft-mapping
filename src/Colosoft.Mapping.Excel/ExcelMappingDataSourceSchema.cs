using System.Collections.Generic;

namespace Colosoft.Mapping.Excel
{
    internal class ExcelMappingDataSourceSchema : IMappingDataSourceSchema
    {
        public ExcelMappingDataSourceSchema(IEnumerable<IMappingDataSourceSchemaField> fields)
        {
            this.Fields = fields;
        }

        public IEnumerable<IMappingDataSourceSchemaField> Fields { get; }
    }
}
