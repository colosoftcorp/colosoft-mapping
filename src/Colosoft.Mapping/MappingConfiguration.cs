using System.Collections.Generic;

namespace Colosoft.Mapping
{
    internal class MappingConfiguration : IMappingConfiguration
    {
        public MappingConfiguration(
            string name,
            IEnumerable<IMappingConfigurationField> fields,
            IMappingDataSourceSchema schema)
        {
            this.Name = name;
            this.Fields = fields;
            this.Schema = schema;
        }

        public IEnumerable<IMappingConfigurationField> Fields { get; }

        public IMappingDataSourceSchema Schema { get; }

        public string Name { get;  }
    }
}
