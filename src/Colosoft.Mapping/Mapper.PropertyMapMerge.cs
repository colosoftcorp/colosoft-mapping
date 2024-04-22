using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Colosoft.Mapping
{
    public partial class Mapper<TSource, TTarget>
    {
        private sealed class PropertyMapMerge<TSourceItem, TTargetItem, TKey> : IPropertyMapAsync<TSource, TTarget>
        {
            private readonly Func<TSource, IEnumerable<TSourceItem>> sourceSelector;
            private readonly Func<TSourceItem, TKey> sourceKeySelector;
            private readonly Func<TTarget, ICollection<TTargetItem>> targetSelector;
            private readonly Func<TTargetItem, TKey> targetKeySelector;
            private readonly Func<TSourceItem, CancellationToken, Task<TTargetItem>> targetFactory;
            private readonly Func<TSourceItem, TTargetItem, CancellationToken, Task> update;
            private readonly IEqualityComparer<TKey> keyComparer;

            public PropertyMapMerge(
                string propertyName,
                Func<TSource, IEnumerable<TSourceItem>> sourceSelector,
                Func<TSourceItem, TKey> sourceKeySelector,
                Func<TTarget, ICollection<TTargetItem>> targetSelector,
                Func<TTargetItem, TKey> targetKeySelector,
                Func<TSourceItem, CancellationToken, Task<TTargetItem>> targetFactory,
                Func<TSourceItem, TTargetItem, CancellationToken, Task> updateAsync,
                IEqualityComparer<TKey> keyComparer)
            {
                this.PropertyName = propertyName;
                this.sourceSelector = sourceSelector;
                this.sourceKeySelector = sourceKeySelector;
                this.targetSelector = targetSelector;
                this.targetKeySelector = targetKeySelector;
                this.targetFactory = targetFactory;
                this.update = updateAsync;
                this.keyComparer = keyComparer;
            }

            public string PropertyName { get; }

            private async Task<TTargetItem> Create(TSourceItem item, CancellationToken cancellationToken)
            {
                var target = await this.targetFactory(item, cancellationToken);

                await this.Update(item, target, cancellationToken);

                return target;
            }

            private Task Update(TSourceItem sourceItem, TTargetItem targetItem, CancellationToken cancellationToken)
            {
                if (this.update != null)
                {
                    return this.update(sourceItem, targetItem, cancellationToken);
                }

                return Task.CompletedTask;
            }

            public void Apply(TSource source, TTarget target) =>
                Task.Run(() => this.ApplyAsync(source, target, null, default)).Wait();

            public Task ApplyAsync(TSource source, TTarget target, IMappingContext context, CancellationToken cancellationToken)
            {
                var sourceValue = this.sourceSelector(source);
                var targetValue = this.targetSelector(target);

                return targetValue.MergeToAsync(
                    sourceValue,
                    this.targetKeySelector,
                    this.sourceKeySelector,
                    this.Create,
                    this.Update,
                    this.keyComparer,
                    cancellationToken);
            }
        }
    }
}
