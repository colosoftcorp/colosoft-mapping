using System.Threading;
using System.Threading.Tasks;
using System.Xml.XPath;

namespace Colosoft.Mapping.Xml
{
    public class XmlMappingDataSourcePreparerFactory : IXmlMappingDataSourcePreparerFactory
    {
        private readonly IXmlMappingDataSourceImporter importer;

        public XmlMappingDataSourcePreparerFactory(IXmlMappingDataSourceImporter importer)
        {
            this.importer = importer;
        }

        public async Task<IMappingDataSourcePreparer> Create(string name, IXPathNavigable styleSheet, CancellationToken cancellationToken)
        {
            await this.importer.Import(styleSheet, cancellationToken);
            return new XmlMappingDataSourcePreparer(name);
        }
    }
}
