using System.Threading;
using System.Threading.Tasks;

namespace Colosoft.Mapping
{
    public interface IMappingDataSourceRecordApplier<TTarget>
    {
        Task Apply(IMappingDataSourceRecord record, TTarget target, IMappingContext context, CancellationToken cancellationToken);
    }
}
