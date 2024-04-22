using System;
using System.Threading;
using System.Threading.Tasks;

namespace Colosoft.Mapping
{
    public partial class Mapper<TSource, TTarget>
    {
        private sealed class PropertyMapAsync<TPropertyValue> : IPropertyMapAsync<TSource, TTarget>
        {
            private readonly Func<TSource, TPropertyValue> getter;
            private readonly Func<TTarget, TPropertyValue, CancellationToken, Task> setter;

            public string PropertyName { get; }

            public PropertyMapAsync(
                string propertyName,
                Func<TSource, TPropertyValue> getter,
                Func<TTarget, TPropertyValue, CancellationToken, Task> setter)
            {
                this.PropertyName = propertyName;
                this.getter = getter;
                this.setter = setter;
            }

            public void Apply(TSource source, TTarget target)
            {
                Task.Run(() => this.ApplyAsync(source, target, null, default)).Wait();
            }

            public async Task ApplyAsync(TSource source, TTarget target, IMappingContext context, CancellationToken cancellationToken)
            {
                var value = this.getter(source);
                await this.setter(target, value, cancellationToken);
            }
        }

        private sealed class PropertyMapAsync<TPropertyValue, TContext> : IPropertyMapContextSupport<TSource, TTarget>
            where TContext : IMappingContext
        {
            private readonly Func<TSource, TPropertyValue> getter;
            private readonly Func<TTarget, TPropertyValue, TContext, CancellationToken, Task> setter;

            public string PropertyName { get; }

            public PropertyMapAsync(
                string propertyName,
                Func<TSource, TPropertyValue> getter,
                Func<TTarget, TPropertyValue, TContext, CancellationToken, Task> setter)
            {
                this.PropertyName = propertyName;
                this.getter = getter;
                this.setter = setter;
            }

            public void Apply(TSource source, TTarget target) =>
                Task.Run(() => this.ApplyAsync(source, target, default)).Wait();

            public void Apply(TSource source, TTarget target, IMappingContext context) =>
                Task.Run(() => this.ApplyAsync(source, target, context, default)).Wait();

            public async Task ApplyAsync(TSource source, TTarget target, CancellationToken cancellationToken)
            {
                var value = this.getter(source);
                await this.setter(target, value, default(TContext), cancellationToken);
            }

            public async Task ApplyAsync(TSource source, TTarget target, IMappingContext context, CancellationToken cancellationToken)
            {
                var value = this.getter(source);
                await this.setter(target, value, (TContext)context, cancellationToken);
            }
        }
    }
}
