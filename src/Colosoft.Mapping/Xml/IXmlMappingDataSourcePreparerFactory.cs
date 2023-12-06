using System.Threading;
using System.Threading.Tasks;
using System.Xml.XPath;

namespace Colosoft.Mapping.Xml
{
    public interface IXmlMappingDataSourcePreparerFactory
    {
        Task<IMappingDataSourcePreparer> Create(string name, IXPathNavigable styleSheet, CancellationToken cancellationToken);
    }
}
