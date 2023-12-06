using System;
using System.Collections.Generic;

namespace Colosoft.Mapping
{
    public class MappingDataSourceSchemaFieldBuilder
    {
        private readonly string name;
        private readonly Type type;
        private string description;
        private string defaultValue;
        private Func<IEnumerable<string>> optionValues;
        private Func<bool> visibilityValues;
        private bool isOptional = true;

        public MappingDataSourceSchemaFieldBuilder(string name, Type type)
        {
            this.name = name;
            this.type = type;
        }

        public MappingDataSourceSchemaFieldBuilder Description(string description)
        {
            this.description = description;
            return this;
        }

        public MappingDataSourceSchemaFieldBuilder DefaultValue(string defaultValue)
        {
            this.defaultValue = defaultValue;
            return this;
        }

        public MappingDataSourceSchemaFieldBuilder OptionValues(Func<IEnumerable<string>> func)
        {
            this.optionValues = func;
            return this;
        }

        public MappingDataSourceSchemaFieldBuilder VisibilityValues(Func<bool> func)
        {
            this.visibilityValues = func;
            return this;
        }

        public MappingDataSourceSchemaFieldBuilder IsRequired()
        {
            this.isOptional = false;
            return this;
        }

        public IMappingDataSourceSchemaField Build() =>
            new Field
            {
                Name = this.name,
                Description = this.description,
                Type = this.type,
                IsOptional = this.isOptional,
                DefaultValue = this.defaultValue,
                OptionsGetter = this.optionValues,
                VisibilityGetter = this.visibilityValues,
            };

        private class Field : IMappingDataSourceSchemaField
        {
            public string Name { get; set; }

            public string Description { get; set; }

            public Type Type { get; set; }

            public bool IsOptional { get; set; }

            public string DefaultValue { get; set; }

            public Func<IEnumerable<string>> OptionsGetter { get; set; }

            public Func<bool> VisibilityGetter { get; set; }
        }
    }
}
