using Colosoft.Mapping.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Colosoft.Mapping
{
    public class Mapper<TSource, TTarget> : IMapperContextSupport<TSource, TTarget>, IMappingContextSupport
    {
        private readonly List<IPropertyMap<TSource, TTarget>> properties = new List<IPropertyMap<TSource, TTarget>>();

        public IEnumerable<IPropertyMap<TSource, TTarget>> Properties => this.properties.AsEnumerable();

        public Mapper<TSource, TTarget> Map<TPropertyValue>(
            Expression<Func<TSource, TPropertyValue>> sourceProperty,
            Expression<Func<TTarget, TPropertyValue>> targetProperty)
        {
            if (sourceProperty == null)
            {
                throw new ArgumentNullException(nameof(sourceProperty));
            }

            if (targetProperty == null)
            {
                throw new ArgumentNullException(nameof(targetProperty));
            }

            var sourceMember = sourceProperty.GetMember();

            if (sourceMember == null)
            {
                throw new ArgumentException("Property undefined", nameof(sourceProperty));
            }

            var getter = sourceProperty.Compile();

            var property = targetProperty.GetMember() as System.Reflection.PropertyInfo;

            if (property.CanWrite)
            {
                var setter = new Action<TTarget, TPropertyValue>((parent, child) =>
                {
                    if (parent == null)
                    {
                        throw new ArgumentNullException(nameof(parent));
                    }

                    property.SetValue(parent, child, null);
                });

                var propertyMap = new PropertyMap<TPropertyValue>(sourceMember.Name, getter, setter);
                this.properties.Add(propertyMap);
                return this;
            }
            else
            {
                throw new InvalidOperationException($"Property {targetProperty.Name} from type {typeof(TTarget).FullName} is read only");
            }
        }

        public Mapper<TSource, TTarget> Map<TPropertyValue>(
            Expression<Func<TSource, TPropertyValue>> sourceProperty,
            Action<TTarget, TPropertyValue> setter)
        {
            if (sourceProperty == null)
            {
                throw new ArgumentNullException(nameof(sourceProperty));
            }

            var sourceMember = sourceProperty.GetMember();

            if (sourceMember == null)
            {
                throw new ArgumentException("Property undefined", nameof(sourceProperty));
            }

            var getter = sourceProperty.Compile();

            var propertyMap = new PropertyMap<TPropertyValue>(sourceMember.Name, getter, setter);
            this.properties.Add(propertyMap);
            return this;
        }

        public Mapper<TSource, TTarget> Map<TPropertyValue, TContext>(
            Expression<Func<TSource, TPropertyValue>> sourceProperty,
            Action<TTarget, TPropertyValue, TContext> setter)
            where TContext : IMappingContext
        {
            if (sourceProperty == null)
            {
                throw new ArgumentNullException(nameof(sourceProperty));
            }

            var sourceMember = sourceProperty.GetMember();

            if (sourceMember == null)
            {
                throw new ArgumentException("Property undefined", nameof(sourceProperty));
            }

            var getter = sourceProperty.Compile();

            var propertyMap = new PropertyMap<TPropertyValue, TContext>(sourceMember.Name, getter, setter);
            this.properties.Add(propertyMap);
            return this;
        }

        public Mapper<TSource, TTarget> MapAsync<TPropertyValue>(
            Expression<Func<TSource, TPropertyValue>> sourceProperty,
            Func<TTarget, TPropertyValue, CancellationToken, Task> setter)
        {
            if (sourceProperty == null)
            {
                throw new ArgumentNullException(nameof(sourceProperty));
            }

            var sourceMember = sourceProperty.GetMember();

            if (sourceMember == null)
            {
                throw new ArgumentException("Property undefined", nameof(sourceProperty));
            }

            var getter = sourceProperty.Compile();

            var propertyMap = new PropertyMapAsync<TPropertyValue>(sourceMember.Name, getter, setter);
            this.properties.Add(propertyMap);
            return this;
        }

        public Mapper<TSource, TTarget> MapAsync<TPropertyValue, TContext>(
            Expression<Func<TSource, TPropertyValue>> sourceProperty,
            Func<TTarget, TPropertyValue, TContext, CancellationToken, Task> setter)
            where TContext : IMappingContext
        {
            if (sourceProperty == null)
            {
                throw new ArgumentNullException(nameof(sourceProperty));
            }

            var sourceMember = sourceProperty.GetMember();

            if (sourceMember == null)
            {
                throw new ArgumentException("Property undefined", nameof(sourceProperty));
            }

            var getter = sourceProperty.Compile();

            var propertyMap = new PropertyMapAsync<TPropertyValue, TContext>(sourceMember.Name, getter, setter);
            this.properties.Add(propertyMap);
            return this;
        }

        public async virtual Task Apply(TSource source, TTarget target, CancellationToken cancellationToken)
        {
            foreach (var property in this.GetChangedProperties(source))
            {
                var propertyMapAsync = property as IPropertyMapAsync<TSource, TTarget>;

                if (propertyMapAsync != null)
                {
                    await propertyMapAsync.ApplyAsync(source, target, null, cancellationToken);
                }
                else
                {
                    property.Apply(source, target);
                }
            }
        }

        public async virtual Task Apply(TSource source, TTarget target, IMappingContext context, CancellationToken cancellationToken)
        {
            foreach (var property in this.GetChangedProperties(source))
            {
                var propertyMapAsync = property as IPropertyMapAsync<TSource, TTarget>;

                if (propertyMapAsync != null)
                {
                    if (propertyMapAsync is IPropertyMapContextSupportAsync<TSource, TTarget>)
                    {
                        await (propertyMapAsync as IPropertyMapContextSupportAsync<TSource, TTarget>).ApplyAsync(source, target, context, cancellationToken);
                    }
                    else
                    {
                        await propertyMapAsync.ApplyAsync(source, target, context, cancellationToken);
                    }
                }
                else if (property is IPropertyMapContextSupport<TSource, TTarget>)
                {
                    (property as IPropertyMapContextSupport<TSource, TTarget>).Apply(source, target, context);
                }
                else
                {
                    property.Apply(source, target);
                }
            }
        }

        Task IMapper.Apply(object source, object target, CancellationToken cancellationToken) =>
            this.Apply((TSource)source, (TTarget)target, cancellationToken);

        Task IMappingContextSupport.Apply(object source, object target, IMappingContext context, CancellationToken cancellationToken) =>
            this.Apply((TSource)source, (TTarget)target, context, cancellationToken);

        protected virtual IEnumerable<IPropertyMap<TSource, TTarget>> GetChangedProperties(TSource source) => this.properties;

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
