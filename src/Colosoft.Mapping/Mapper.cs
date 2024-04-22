using Colosoft.Mapping.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Colosoft.Mapping
{
    public partial class Mapper<TSource, TTarget> : IMapperContextSupport<TSource, TTarget>, IMappingContextSupport
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

        public Mapper<TSource, TTarget> Map<TSourceItem, TTargetItem, TKey>(
            Expression<Func<TSource, IEnumerable<TSourceItem>>> sourceProperty,
            Func<TSourceItem, TKey> sourceKeySelector,
            Expression<Func<TTarget, ICollection<TTargetItem>>> targetProperty,
            Func<TTargetItem, TKey> targetKeySelector,
            Func<TSourceItem, TTargetItem> targetFactory,
            Action<TSourceItem, TTargetItem> update,
            IEqualityComparer<TKey> keyComparer = null)
        {
            if (sourceProperty is null)
            {
                throw new ArgumentNullException(nameof(sourceProperty));
            }

            if (sourceKeySelector is null)
            {
                throw new ArgumentNullException(nameof(sourceKeySelector));
            }

            if (targetProperty is null)
            {
                throw new ArgumentNullException(nameof(targetProperty));
            }

            if (targetKeySelector is null)
            {
                throw new ArgumentNullException(nameof(targetKeySelector));
            }

            if (targetFactory is null)
            {
                throw new ArgumentNullException(nameof(targetFactory));
            }

            if (update is null)
            {
                throw new ArgumentNullException(nameof(update));
            }

            return this.Map(
                sourceProperty,
                sourceKeySelector,
                targetProperty,
                targetKeySelector,
                (f, _) => Task.FromResult(targetFactory(f)),
                (x, y, _) =>
                {
                    update(x, y);
                    return Task.CompletedTask;
                },
                keyComparer);
        }

        public Mapper<TSource, TTarget> Map<TSourceItem, TTargetItem, TKey>(
            Expression<Func<TSource, IEnumerable<TSourceItem>>> sourceProperty,
            Func<TSourceItem, TKey> sourceKeySelector,
            Expression<Func<TTarget, ICollection<TTargetItem>>> targetProperty,
            Func<TTargetItem, TKey> targetKeySelector,
            Func<TSourceItem, TTargetItem> targetFactory,
            Func<TSourceItem, TTargetItem, CancellationToken, Task> update,
            IEqualityComparer<TKey> keyComparer = null)
        {
            if (sourceProperty is null)
            {
                throw new ArgumentNullException(nameof(sourceProperty));
            }

            if (sourceKeySelector is null)
            {
                throw new ArgumentNullException(nameof(sourceKeySelector));
            }

            if (targetProperty is null)
            {
                throw new ArgumentNullException(nameof(targetProperty));
            }

            if (targetKeySelector is null)
            {
                throw new ArgumentNullException(nameof(targetKeySelector));
            }

            if (targetFactory is null)
            {
                throw new ArgumentNullException(nameof(targetFactory));
            }

            if (update is null)
            {
                throw new ArgumentNullException(nameof(update));
            }

            return this.Map(
                sourceProperty,
                sourceKeySelector,
                targetProperty,
                targetKeySelector,
                (f, _) => Task.FromResult(targetFactory(f)),
                update,
                keyComparer);
        }

        public Mapper<TSource, TTarget> Map<TSourceItem, TTargetItem, TKey>(
            Expression<Func<TSource, IEnumerable<TSourceItem>>> sourceProperty,
            Func<TSourceItem, TKey> sourceKeySelector,
            Expression<Func<TTarget, ICollection<TTargetItem>>> targetProperty,
            Func<TTargetItem, TKey> targetKeySelector,
            Func<CancellationToken, Task<TTargetItem>> targetFactory,
            Func<TSourceItem, TTargetItem, CancellationToken, Task> update,
            IEqualityComparer<TKey> keyComparer = null)
        {
            if (sourceProperty is null)
            {
                throw new ArgumentNullException(nameof(sourceProperty));
            }

            if (sourceKeySelector is null)
            {
                throw new ArgumentNullException(nameof(sourceKeySelector));
            }

            if (targetProperty is null)
            {
                throw new ArgumentNullException(nameof(targetProperty));
            }

            if (targetKeySelector is null)
            {
                throw new ArgumentNullException(nameof(targetKeySelector));
            }

            if (targetFactory is null)
            {
                throw new ArgumentNullException(nameof(targetFactory));
            }

            if (update is null)
            {
                throw new ArgumentNullException(nameof(update));
            }

            return this.Map(
                sourceProperty,
                sourceKeySelector,
                targetProperty,
                targetKeySelector,
                (_, cancellationToken) => targetFactory(cancellationToken),
                update,
                keyComparer);
        }

        public Mapper<TSource, TTarget> Map<TSourceItem, TTargetItem, TKey>(
            Expression<Func<TSource, IEnumerable<TSourceItem>>> sourceProperty,
            Func<TSourceItem, TKey> sourceKeySelector,
            Expression<Func<TTarget, ICollection<TTargetItem>>> targetProperty,
            Func<TTargetItem, TKey> targetKeySelector,
            Func<TSourceItem, CancellationToken, Task<TTargetItem>> targetFactory,
            Func<TSourceItem, TTargetItem, CancellationToken, Task> update,
            IEqualityComparer<TKey> keyComparer = null)
        {
            if (sourceProperty == null)
            {
                throw new ArgumentNullException(nameof(sourceProperty));
            }

            if (sourceKeySelector is null)
            {
                throw new ArgumentNullException(nameof(sourceKeySelector));
            }

            if (targetProperty is null)
            {
                throw new ArgumentNullException(nameof(targetProperty));
            }

            if (targetKeySelector is null)
            {
                throw new ArgumentNullException(nameof(targetKeySelector));
            }

            if (targetFactory is null)
            {
                throw new ArgumentNullException(nameof(targetFactory));
            }

            if (update is null)
            {
                throw new ArgumentNullException(nameof(update));
            }

            var sourceMember = sourceProperty.GetMember();

            if (sourceMember == null)
            {
                throw new ArgumentException("Property undefined", nameof(sourceProperty));
            }

            var targetMember = targetProperty.GetMember();

            if (targetMember == null)
            {
                throw new ArgumentException("Property undefined", nameof(targetProperty));
            }

            var sourceSelector = sourceProperty.Compile();
            var targetSelector = targetProperty.Compile();

            var propertyMap = new PropertyMapMerge<TSourceItem, TTargetItem, TKey>(
                sourceMember.Name,
                sourceSelector,
                sourceKeySelector,
                targetSelector,
                targetKeySelector,
                targetFactory,
                update,
                keyComparer);

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
    }
}
