using System.Collections.Generic;

namespace Colosoft.Mapping
{
    public interface IMappingConfiguration
    {
        IEnumerable<IMappingConfigurationField> Fields { get; }

        IMappingDataSourceSchema Schema { get; }

        string Name { get; }
    }
}
