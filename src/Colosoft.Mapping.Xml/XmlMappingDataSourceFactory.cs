using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace Colosoft.Mapping.Xml
{
    public class XmlMappingDataSourceFactory : IXmlMappingDataSourceFactory
    {
        private const string Extension = ".xml";

        public Task<bool> IsCompatible(string fileExtension, Func<CancellationToken, Task<Stream>> contentGetter, CancellationToken cancellationToken) =>
            Task.FromResult(Extension.Equals(fileExtension, StringComparison.InvariantCultureIgnoreCase));

        public async Task<IXmlMappingDataSource> Create(IMappingDataSource dataSource, CancellationToken cancellationToken)
        {
            var document = new XmlDocument();
            var xmlDeclaration = document.CreateXmlDeclaration("1.0", "UTF-8", null);
            var root = document.DocumentElement;
            document.InsertBefore(xmlDeclaration, root);

            var body = document.CreateElement(string.Empty, "items", string.Empty);
            document.AppendChild(body);

            foreach (var record in await dataSource.GetRecords(cancellationToken))
            {
                var xmlElement = document.CreateElement(string.Empty, "item", string.Empty);
                xmlElement.SetAttribute("datasource-name", dataSource.Name);
                body.AppendChild(xmlElement);
                foreach (var field in record.Schema.Fields)
                {
                    var element = document.CreateElement(string.Empty, field.Name, string.Empty);
                    var value = record.GetValue(field.Name);
                    var text = string.Empty;
                    if (value != null)
                    {
                        if (value is string textValue)
                        {
                            text = textValue;
                        }
                        else if (value is IConvertible convertible)
                        {
                            text = convertible.ToString(System.Globalization.CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            text = value.ToString();
                        }
                    }

                    var textNode = document.CreateTextNode(text);
                    element.AppendChild(textNode);
                    xmlElement.AppendChild(element);
                }
            }

            return new XmlMappingDataSource(document);
        }

        public Task<IMappingDataSource> Create(Stream inputStream, CancellationToken cancellationToken)
        {
            var document = new XmlDocument();
            using (var reader = XmlReader.Create(inputStream))
            {
                document.Load(reader);
            }

            return Task.FromResult((IMappingDataSource)new XmlMappingDataSource(document));
        }
    }
}
