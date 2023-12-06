using System.Threading;
using System.Threading.Tasks;

namespace Colosoft.Mapping
{
    public interface IMappingPreparer
    {
        string Name { get; }

        bool IsCompatibillity(IMappingDataSource source);

        Task<IMappingDataSource> Prepare(IMappingDataSource source, CancellationToken cancellationToken);
    }
}
