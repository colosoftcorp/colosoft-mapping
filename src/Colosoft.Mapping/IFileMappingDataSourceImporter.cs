using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Colosoft.Mapping
{
    public interface IFileMappingDataSourceImporter
    {
        Task<bool> IsCompatible(string fileExtension, Func<CancellationToken, Task<Stream>> contentGetter, CancellationToken cancellationToken);

        Task<IEnumerable<IMappingDataSource>> Import(Stream inputStream, CancellationToken cancellationToken);
    }
}
