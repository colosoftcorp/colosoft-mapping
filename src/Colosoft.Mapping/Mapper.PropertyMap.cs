using System;

namespace Colosoft.Mapping
{
    public partial class Mapper<TSource, TTarget>
    {
        private sealed class PropertyMap<TPropertyValue, TContext> : IPropertyMapContextSupport<TSource, TTarget>
            where TContext : IMappingContext
        {
            private readonly Func<TSource, TPropertyValue> getter;
            private readonly Action<TTarget, TPropertyValue, TContext> setter;

            public string PropertyName { get; }

            public PropertyMap(
                string propertyName,
                Func<TSource, TPropertyValue> getter,
                Action<TTarget, TPropertyValue, TContext> setter)
            {
                this.PropertyName = propertyName;
                this.getter = getter;
                this.setter = setter;
            }

            public void Apply(TSource source, TTarget target)
            {
                var value = this.getter(source);
                this.setter(target, value, default(TContext));
            }

            public void Apply(TSource source, TTarget target, IMappingContext context)
            {
                var value = this.getter(source);
                this.setter(target, value, (TContext)context);
            }
        }

        private sealed class PropertyMap<TPropertyValue> : IPropertyMapContextSupport<TSource, TTarget>
        {
            private readonly Func<TSource, TPropertyValue> getter;
            private readonly Action<TTarget, TPropertyValue> setter;

            public string PropertyName { get; }

            public PropertyMap(
                string propertyName,
                Func<TSource, TPropertyValue> getter,
                Action<TTarget, TPropertyValue> setter)
            {
                this.PropertyName = propertyName;
                this.getter = getter;
                this.setter = setter;
            }

            public void Apply(TSource source, TTarget target) =>
                this.Apply(source, target, null);

            public void Apply(TSource source, TTarget target, IMappingContext context)
            {
                var value = this.getter(source);
                this.setter(target, value);
            }
        }
    }
}
