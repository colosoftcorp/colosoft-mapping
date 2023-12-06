using Colosoft.Mapping.Expressions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Colosoft.Mapping
{
    public class MappingDataSourceSchemaFieldBuilder<TTarget, TPropertyValue> : IMappingDataSourceSchemaFieldBuilder
    {
        private readonly string name;
        private readonly IMappingConfigurationBuilder<TTarget> mappingConfigurationBuilder;
        private string description;
        private string defaultValue;
        private Func<IEnumerable<string>> optionValues;
        private Func<bool> visibilityValues;
        private bool isOptional = true;

        public MappingDataSourceSchemaFieldBuilder(
            string name,
            IMappingConfigurationBuilder<TTarget> mappingConfigurationBuilder)
        {
            this.name = name;
            this.mappingConfigurationBuilder = mappingConfigurationBuilder;
        }

        public MappingDataSourceSchemaFieldBuilder<TTarget, TPropertyValue> Description(string description)
        {
            this.description = description;
            return this;
        }

        public MappingDataSourceSchemaFieldBuilder<TTarget, TPropertyValue> DefaultValue(string defaultValue)
        {
            this.defaultValue = defaultValue;
            return this;
        }

        public MappingDataSourceSchemaFieldBuilder<TTarget, TPropertyValue> OptionValues(Func<IEnumerable<string>> func)
        {
            this.optionValues = func;
            return this;
        }

        public MappingDataSourceSchemaFieldBuilder<TTarget, TPropertyValue> VisibilityValues(Func<bool> func)
        {
            this.visibilityValues = func;
            return this;
        }

        public MappingDataSourceSchemaFieldBuilder<TTarget, TPropertyValue> IsRequired()
        {
            this.isOptional = false;
            return this;
        }

        public MappingDataSourceSchemaFieldBuilder<TTarget, TPropertyValue> Map(Action<TTarget, TPropertyValue> setter)
        {
            this.mappingConfigurationBuilder.Map(this.name, setter);
            return this;
        }

        public MappingDataSourceSchemaFieldBuilder<TTarget, TPropertyValue> Map<TContext>(Action<TTarget, TPropertyValue, TContext> setter)
            where TContext : IMappingContext
        {
            this.mappingConfigurationBuilder.Map(this.name, setter);
            return this;
        }

        public MappingDataSourceSchemaFieldBuilder<TTarget, TPropertyValue> Map(Expression<Func<TTarget, TPropertyValue>> expression)
        {
            var setter = ExpressionExtensions.GetSetter(expression);
            this.mappingConfigurationBuilder.Map(this.name, setter);
            return this;
        }

        public MappingDataSourceSchemaFieldBuilder<TTarget, TPropertyValue> MapAsync<TContext>(Func<TTarget, TPropertyValue, TContext, Task> setter)
            where TContext : IMappingContext
        {
            this.mappingConfigurationBuilder.MapAsync(this.name, setter);
            return this;
        }

        public IMappingDataSourceSchemaField Build() =>
            new Field
            {
                Name = this.name,
                Description = this.description,
                Type = typeof(TPropertyValue),
                IsOptional = this.isOptional,
                DefaultValue = this.defaultValue,
                OptionsGetter = this.optionValues,
                VisibilityGetter = this.visibilityValues,
            };

        private sealed class Field : IMappingDataSourceSchemaField
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
