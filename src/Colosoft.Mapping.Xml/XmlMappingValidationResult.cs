using System.Collections.Generic;

namespace Colosoft.Mapping.Xml
{
    public class XmlMappingValidationResult : IMappingValidationResult
    {
        public XmlMappingValidationResult(
            bool success,
            IEnumerable<IMappingValidationResultItem> items)
        {
            this.Success = success;
            this.Items = items;
        }

        public bool Success { get; }

        public IEnumerable<IMappingValidationResultItem> Items { get; }
    }
}
