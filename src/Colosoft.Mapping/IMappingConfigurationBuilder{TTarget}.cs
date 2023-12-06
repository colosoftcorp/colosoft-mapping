using System;
using System.Threading.Tasks;

namespace Colosoft.Mapping
{
    public interface IMappingConfigurationBuilder<TTarget> : IMappingConfigurationBuilder
    {
        IMappingConfigurationBuilder<TTarget> Map<TPropertyValue>(string name, Action<TTarget, TPropertyValue> setter);

        IMappingConfigurationBuilder<TTarget> Map<TPropertyValue, TContext>(string name, Action<TTarget, TPropertyValue, TContext> setter)
            where TContext : IMappingContext;

        IMappingConfigurationBuilder<TTarget> Map(string name, Action<TTarget, IMappingDataSourceRecord> setter);

        Task<IMappingConfigurationBuilder<TTarget>> MapAsync<TPropertyValue>(string name, Func<TTarget, TPropertyValue, Task> setter);

        Task<IMappingConfigurationBuilder<TTarget>> MapAsync<TPropertyValue, TContext>(string name, Func<TTarget, TPropertyValue, TContext, Task> setter)
            where TContext : IMappingContext;

        Task<IMappingConfigurationBuilder<TTarget>> MapAsync(string name, Func<TTarget, IMappingDataSourceRecord, Task> setter);

        void Schema(IMappingDataSourceSchema schema);
    }
}
