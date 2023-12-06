using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Colosoft.Mapping
{
    internal class MappingConfigurationBuilder<TTarget> : IMappingConfigurationBuilder<TTarget>
    {
        private readonly List<IMappingConfigurationField> fields = new List<IMappingConfigurationField>();

        private string name;
        private IMappingDataSourceSchema sourceSchema;

        public IMappingConfiguration Build()
        {
            return new MappingConfiguration(
                this.name ?? typeof(TTarget).Name,
                this.fields,
                this.sourceSchema);
        }

        public IMappingConfigurationBuilder<TTarget> Name(string name)
        {
            this.name = name;
            return this;
        }

        public IMappingConfigurationBuilder<TTarget> Map<TPropertyValue>(string name, Action<TTarget, TPropertyValue> setter)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }
            else if (setter == null)
            {
                throw new ArgumentNullException(nameof(setter));
            }

            this.fields.Add(new MappingConfigurationField<TTarget, TPropertyValue, IMappingContext>(
                name,
                (target, value, context) =>
                {
                    setter(target, value);
                    return Task.FromResult(true);
                }));
            return this;
        }

        public IMappingConfigurationBuilder<TTarget> Map<TPropertyValue, TContext>(string name, Action<TTarget, TPropertyValue, TContext> setter)
             where TContext : IMappingContext
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }
            else if (setter == null)
            {
                throw new ArgumentNullException(nameof(setter));
            }

            this.fields.Add(new MappingConfigurationField<TTarget, TPropertyValue, IMappingContext>(
                name,
                (target, value, context) =>
                {
                    setter(target, value, (TContext)context);
                    return Task.FromResult(true);
                }));
            return this;
        }

        public IMappingConfigurationBuilder<TTarget> Map(string name, Action<TTarget, IMappingDataSourceRecord> setter)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }
            else if (setter == null)
            {
                throw new ArgumentNullException(nameof(setter));
            }

            this.fields.Add(new MappingConfigurationField<TTarget, IMappingDataSourceRecord, IMappingContext>(
                name,
                (target, value, context) =>
                {
                    setter(target, value);
                    return Task.FromResult(true);
                }));
            return this;
        }

        public Task<IMappingConfigurationBuilder<TTarget>> MapAsync<TPropertyValue>(string name, Func<TTarget, TPropertyValue, Task> setter)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }
            else if (setter == null)
            {
                throw new ArgumentNullException(nameof(setter));
            }

            this.fields.Add(new MappingConfigurationField<TTarget, TPropertyValue, IMappingContext>(
                name,
                (target, value, context) =>
                {
                    setter(target, value);
                    return Task.FromResult(true);
                }));

            return Task.FromResult((IMappingConfigurationBuilder<TTarget>)this);
        }

        public Task<IMappingConfigurationBuilder<TTarget>> MapAsync<TPropertyValue, TContext>(string name, Func<TTarget, TPropertyValue, TContext, Task> setter)
            where TContext : IMappingContext
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }
            else if (setter == null)
            {
                throw new ArgumentNullException(nameof(setter));
            }

            this.fields.Add(new MappingConfigurationField<TTarget, TPropertyValue, IMappingContext>(
                name,
                (target, value, context) =>
                {
                    setter(target, value, (TContext)context);
                    return Task.FromResult(true);
                }));

            return Task.FromResult((IMappingConfigurationBuilder<TTarget>)this);
        }

        public Task<IMappingConfigurationBuilder<TTarget>> MapAsync(string name, Func<TTarget, IMappingDataSourceRecord, Task> setter)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }
            else if (setter == null)
            {
                throw new ArgumentNullException(nameof(setter));
            }

            this.fields.Add(new MappingConfigurationField<TTarget, IMappingDataSourceRecord, IMappingContext>(
                name,
                (target, value, context) =>
                {
                    setter(target, value);
                    return Task.FromResult(true);
                }));

            return Task.FromResult((IMappingConfigurationBuilder<TTarget>)this);
        }

        public void Schema(IMappingDataSourceSchema schema)
        {
            this.sourceSchema = schema;
        }
    }
}
