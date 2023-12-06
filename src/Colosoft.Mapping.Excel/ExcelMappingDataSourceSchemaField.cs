using System;
using System.Collections.Generic;

namespace Colosoft.Mapping.Excel
{
    internal class ExcelMappingDataSourceSchemaField : IMappingDataSourceSchemaField
    {
        public string Description { get; set; }

        public string Name { get; set; }

        public Type Type { get; set; }

        public bool IsOptional { get; set; }

        public string DefaultValue { get; set; }

        public Func<IEnumerable<string>> OptionsGetter { get; set; }

        public Func<bool> VisibilityGetter { get; set; }
    }
}
