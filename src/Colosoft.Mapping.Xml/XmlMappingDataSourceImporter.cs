using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.XPath;

namespace Colosoft.Mapping.Xml
{
    public class XmlMappingDataSourceImporter : IXmlMappingDataSourceImporter
    {
        private readonly IXmlMappingDataSourceFactory dataSourceFactory;

        private readonly string[] extensions = new[] { ".xml" };

        public Task<bool> IsCompatible(string fileExtension, Func<CancellationToken, Task<Stream>> contentGetter, CancellationToken cancellationToken)
        {
            return Task.FromResult(this.extensions.Any(f => f.Equals(fileExtension, StringComparison.InvariantCultureIgnoreCase)));
        }

        public XmlMappingDataSourceImporter(IXmlMappingDataSourceFactory dataSourceFactory)
        {
            this.dataSourceFactory = dataSourceFactory;
        }

        public async Task<IXmlMappingDataSource> Import(TextReader reader, CancellationToken cancellationToken)
        {
            var document = new XmlDocument();
            document.Load(reader);

            return await this.dataSourceFactory.Create(new XmlMappingDataSource(document), cancellationToken);
        }

        public async Task<IXmlMappingDataSource> Import(IXPathNavigable input, CancellationToken cancellationToken)
        {
            return await this.dataSourceFactory.Create(new XmlMappingDataSource(input), cancellationToken);
        }

        public async Task<IEnumerable<IMappingDataSource>> Import(Stream inputStream, CancellationToken cancellationToken)
        {
            return new[] { await this.dataSourceFactory.Create(inputStream, cancellationToken) };
        }
    }
}
