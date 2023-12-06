using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Colosoft.Mapping
{
    public interface IMappingDataSource
    {
        string Name { get; }

        IMappingDataSourceSchema Schema { get; }

        Task<IEnumerable<IMappingDataSourceRecord>> GetRecords(CancellationToken cancellationToken);
    }
}
