using System.Threading;
using System.Threading.Tasks;

namespace Colosoft.Mapping
{
    public interface IMapper
    {
        Task Apply(object source, object target, CancellationToken cancellationToken);
    }
}
