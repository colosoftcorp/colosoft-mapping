using System.Xml.XPath;

namespace Colosoft.Mapping.Xml
{
    public interface IXmlMappingDataSource : IMappingDataSource
    {
        IXPathNavigable Root { get; }
    }
}
