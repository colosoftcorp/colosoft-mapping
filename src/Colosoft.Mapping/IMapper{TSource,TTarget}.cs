using System.Threading;
using System.Threading.Tasks;

namespace Colosoft.Mapping
{
    public interface IMapper<in TSource, in TTarget> : IMapper
    {
        Task Apply(TSource source, TTarget target, CancellationToken cancellationToken);
    }
}
