using System.Threading;
using System.Threading.Tasks;

namespace Colosoft.Mapping
{
    public interface IPropertyMapAsync<in TSource, in TTarget> : IPropertyMap<TSource, TTarget>
    {
        Task ApplyAsync(TSource source, TTarget target, IMappingContext context, CancellationToken cancellationToken);
    }
}
