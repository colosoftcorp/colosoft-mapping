using System.Threading;
using System.Threading.Tasks;

namespace Colosoft.Mapping
{
    public interface IPropertyMapContextSupportAsync<in TSource, in TTarget> :
        IPropertyMapContextSupport<TSource, TTarget>
    {
        Task ApplyAsync(TSource source, TTarget target, IMappingContext context, CancellationToken cancellationToken);
    }
}
