using System.Collections.Generic;
using System.Linq;

namespace Colosoft.Mapping
{
    public class MappingDataSourceSchemaBuilder
    {
        private readonly List<MappingDataSourceSchemaFieldBuilder> fields = new List<MappingDataSourceSchemaFieldBuilder>();

        public MappingDataSourceSchemaFieldBuilder Field<T>(string name)
        {
            var fieldBuilder = new MappingDataSourceSchemaFieldBuilder(name, typeof(T));
            this.fields.Add(fieldBuilder);

            return fieldBuilder;
        }

        public IMappingDataSourceSchema Build() =>
            new Schema(this.fields.Select(f => f.Build()).ToList());

        private class Schema : IMappingDataSourceSchema
        {
            public Schema(IEnumerable<IMappingDataSourceSchemaField> fields)
            {
                this.Fields = fields;
            }

            public IEnumerable<IMappingDataSourceSchemaField> Fields { get; }
        }
    }
}
