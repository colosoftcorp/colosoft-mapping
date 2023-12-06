using System;
using System.Collections.Generic;

namespace Colosoft.Mapping.Xml
{
    internal class XmlMappingDataSourceSchemaField : IMappingDataSourceSchemaField
    {
        public XmlMappingDataSourceSchemaField(string name, Type type)
        {
            this.Name = name;
            this.Type = type;
        }

        public string Name { get; }

        public Type Type { get; }

        public string Description { get; }

        public bool IsOptional { get; }

        public string DefaultValue { get; }

        public Func<IEnumerable<string>> OptionsGetter { get; }

        public Func<bool> VisibilityGetter { get; }
    }
}
