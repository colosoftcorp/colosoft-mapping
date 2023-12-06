using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Colosoft.Mapping
{
    public interface IMappingDataSourceFactory
    {
        Task<bool> IsCompatible(string fileExtension, Func<CancellationToken, Task<Stream>> contentGetter, CancellationToken cancellationToken);

        Task<IMappingDataSource> Create(Stream inputStream, CancellationToken cancellationToken);
    }
}
