using System.Collections.Generic;

namespace Colosoft.Mapping
{
    public interface IMappingDataSourceSchema
    {
        IEnumerable<IMappingDataSourceSchemaField> Fields { get; }
    }
}
