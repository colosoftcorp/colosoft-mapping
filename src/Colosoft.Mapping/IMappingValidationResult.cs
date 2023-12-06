using System.Collections.Generic;

namespace Colosoft.Mapping
{
    public interface IMappingValidationResult
    {
        bool Success { get; }

        IEnumerable<IMappingValidationResultItem> Items { get; }
    }
}
