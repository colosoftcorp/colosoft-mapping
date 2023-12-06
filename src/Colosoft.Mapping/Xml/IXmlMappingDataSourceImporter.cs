using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.XPath;

namespace Colosoft.Mapping.Xml
{
    public interface IXmlMappingDataSourceImporter : IFileMappingDataSourceImporter
    {
        Task<IXmlMappingDataSource> Import(TextReader reader, CancellationToken cancellationToken);

        Task<IXmlMappingDataSource> Import(IXPathNavigable input, CancellationToken cancellationToken);
    }
}
