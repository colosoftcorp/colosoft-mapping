using System.Xml.Xsl;

namespace Colosoft.Mapping.Xml
{
    public interface IXmlMappingTransform
    {
        XsltArgumentList Arguments { get; }
    }
}
