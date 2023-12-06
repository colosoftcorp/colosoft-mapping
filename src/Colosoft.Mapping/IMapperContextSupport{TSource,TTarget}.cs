using System.Threading;
using System.Threading.Tasks;

namespace Colosoft.Mapping
{
    public interface IMapperContextSupport<in TSource, in TTarget> : IMapper<TSource, TTarget>
    {
        Task Apply(TSource source, TTarget target, IMappingContext context, CancellationToken cancellationToken);
    }
}
