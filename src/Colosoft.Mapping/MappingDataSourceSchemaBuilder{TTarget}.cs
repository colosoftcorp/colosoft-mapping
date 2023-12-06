using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Colosoft.Mapping
{
    public class MappingDataSourceSchemaBuilder<TTarget>
    {
        private readonly IMappingConfigurationBuilder<TTarget> mappingConfigurationBuilder;
        private readonly List<IMappingDataSourceSchemaFieldBuilder> fieldBuilders = new List<IMappingDataSourceSchemaFieldBuilder>();

        public MappingDataSourceSchemaBuilder(IMappingConfigurationBuilder<TTarget> mappingConfigurationBuilder)
        {
            this.mappingConfigurationBuilder = mappingConfigurationBuilder;
        }

        public MappingDataSourceSchemaFieldBuilder<TTarget, TPropertyValue> Field<TPropertyValue>(string name)
        {
            var fieldBuilder = new MappingDataSourceSchemaFieldBuilder<TTarget, TPropertyValue>(name, this.mappingConfigurationBuilder);
            this.fieldBuilders.Add(fieldBuilder);
            return fieldBuilder;
        }

        public MappingDataSourceSchemaFieldBuilder<TTarget, TPropertyValue> Field<TPropertyValue>(string name, Expression<Func<TTarget, TPropertyValue>> expression)
        {
            var fieldBuilder = new MappingDataSourceSchemaFieldBuilder<TTarget, TPropertyValue>(name, this.mappingConfigurationBuilder);
            fieldBuilder.Map(expression);
            this.fieldBuilders.Add(fieldBuilder);
            return fieldBuilder;
        }

        public MappingDataSourceSchemaFieldBuilder<TTarget, TPropertyValue> Field<TPropertyValue>(string name, Action<TTarget, TPropertyValue> setter)
        {
            var fieldBuilder = new MappingDataSourceSchemaFieldBuilder<TTarget, TPropertyValue>(name, this.mappingConfigurationBuilder);
            fieldBuilder.Map(setter);
            this.fieldBuilders.Add(fieldBuilder);
            return fieldBuilder;
        }

        public MappingDataSourceSchemaFieldBuilder<TTarget, TPropertyValue> Field<TPropertyValue, TContext>(string name, Action<TTarget, TPropertyValue, TContext> setter)
            where TContext : IMappingContext
        {
            var fieldBuilder = new MappingDataSourceSchemaFieldBuilder<TTarget, TPropertyValue>(name, this.mappingConfigurationBuilder);
            fieldBuilder.Map(setter);
            this.fieldBuilders.Add(fieldBuilder);
            return fieldBuilder;
        }

        public IMappingDataSourceSchema Build() =>
            new Schema(this.fieldBuilders
                .Select(f => f.Build())
                .Where(f => f.VisibilityGetter == null || f.VisibilityGetter.Invoke())
                .ToList());

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
