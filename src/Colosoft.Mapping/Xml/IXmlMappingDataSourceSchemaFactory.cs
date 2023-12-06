using System.Xml.Schema;
using System.Xml.XPath;

namespace Colosoft.Mapping.Xml
{
    public interface IXmlMappingDataSourceSchemaFactory
    {
        IXmlMappingDataSourceSchema Create(IMappingDataSource dataSource);

        IXmlMappingDataSourceSchema Create(XmlSchema xmlSchema);

        IXmlMappingDataSourceSchema Create(IXPathNavigable element);
    }
}
