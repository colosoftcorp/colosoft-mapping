using System.Threading;
using System.Threading.Tasks;

namespace Colosoft.Mapping
{
    public interface IMappingValidator
    {
        Task<IMappingValidationResult> Validate(
            IMappingDataSourceRecord record,
            IMappingContext context,
            CancellationToken cancellationToken);
    }
}
