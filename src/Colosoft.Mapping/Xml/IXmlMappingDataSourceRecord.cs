using System.Xml.XPath;

namespace Colosoft.Mapping.Xml
{
    public interface IXmlMappingDataSourceRecord : IMappingDataSourceRecord
    {
        IXPathNavigable Item { get; }
    }
}
