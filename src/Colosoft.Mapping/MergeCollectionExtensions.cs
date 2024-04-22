using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Colosoft.Mapping
{
    public static class MergeCollectionExtensions
    {
        public static Task MergeToAsync<TTarget, TSource, TKey>(
            this ICollection<TTarget> target,
            IEnumerable<TSource> source,
            Func<TTarget, TKey> targetKeySelector,
            Func<TSource, TKey> sourceKeySelector,
            Func<TSource, CancellationToken, Task<TTarget>> targetFactory,
            Func<TSource, TTarget, CancellationToken, Task> update,
            CancellationToken cancellationToken)
        {
            return MergeToAsync(target, source, targetKeySelector, sourceKeySelector, targetFactory, update, null, cancellationToken);
        }

        public static async Task MergeToAsync<TTarget, TSource, TKey>(
            this ICollection<TTarget> target,
            IEnumerable<TSource> source,
            Func<TTarget, TKey> targetKeySelector,
            Func<TSource, TKey> sourceKeySelector,
            Func<TSource, CancellationToken, Task<TTarget>> targetFactory,
            Func<TSource, TTarget, CancellationToken, Task> update,
            IEqualityComparer<TKey> keyComparer,
            CancellationToken cancellationToken)
        {
            var updateItems = target
                .Join(
                    source,
                    targetKeySelector,
                    sourceKeySelector,
                    (left, right) => new { Left = left, Right = right },
                    keyComparer)
                .ToArray();

            var newItems = source.Except(updateItems.Select(f => f.Right)).ToList();
            var deleteItems = target.Except(updateItems.Select(f => f.Left)).ToArray();

            foreach (var item in deleteItems)
            {
                target.Remove(item);
            }

            foreach (var item in newItems)
            {
                var value = await targetFactory(item, cancellationToken);
                target.Add(value);
            }

            foreach (var item in updateItems)
            {
                await update(item.Right, item.Left, cancellationToken);
            }
        }

        public static void MergeTo<TTarget, TSource, TKey>(
            this ICollection<TTarget> target,
            IEnumerable<TSource> source,
            Func<TTarget, TKey> targetKeySelector,
            Func<TSource, TKey> sourceKeySelector,
            Func<TSource, TTarget> targetFactory,
            Action<TSource, TTarget> update)
        {
            MergeTo(target, source, targetKeySelector, sourceKeySelector, targetFactory, update, null);
        }

        public static void MergeTo<TTarget, TSource, TKey>(
            this ICollection<TTarget> target,
            IEnumerable<TSource> source,
            Func<TTarget, TKey> targetKeySelector,
            Func<TSource, TKey> sourceKeySelector,
            Func<TSource, TTarget> targetFactory,
            Action<TSource, TTarget> update,
            IEqualityComparer<TKey> keyComparer)
        {
            var updateItems = target
                .Join(
                    source,
                    targetKeySelector,
                    sourceKeySelector,
                    (left, right) => new { Left = left, Right = right },
                    keyComparer)
                .ToArray();

            var newItems = source.Except(updateItems.Select(f => f.Right)).ToList();
            var deleteItems = target.Except(updateItems.Select(f => f.Left)).ToArray();

            foreach (var item in deleteItems)
            {
                target.Remove(item);
            }

            foreach (var item in newItems)
            {
                var value = targetFactory(item);
                target.Add(value);
            }

            foreach (var item in updateItems)
            {
                update(item.Right, item.Left);
            }
        }
    }
}
