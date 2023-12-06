using System;
using System.Collections.Generic;

namespace Colosoft.Mapping
{
    public interface IMappingDataSourceSchemaField
    {
        string Name { get; }

        string Description { get; }

        Type Type { get; }

        bool IsOptional { get; }

        string DefaultValue { get; }

        Func<IEnumerable<string>> OptionsGetter { get; }

        Func<bool> VisibilityGetter { get; }
    }
}