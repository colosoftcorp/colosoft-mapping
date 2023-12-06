using System.Threading;
using System.Threading.Tasks;

namespace Colosoft.Mapping.Xml
{
    public interface IXmlMappingDataSourceFactory : IMappingDataSourceFactory
    {
        Task<IXmlMappingDataSource> Create(IMappingDataSource dataSource, CancellationToken cancellationToken);
    }
}
