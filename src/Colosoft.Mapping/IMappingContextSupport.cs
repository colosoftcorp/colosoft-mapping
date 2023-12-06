using System.Threading;
using System.Threading.Tasks;

namespace Colosoft.Mapping
{
    public interface IMappingContextSupport : IMapper
    {
        Task Apply(object source, object target, IMappingContext context, CancellationToken cancellationToken);
    }
}
