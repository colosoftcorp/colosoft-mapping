using System.Collections.Generic;

namespace Colosoft.Mapping
{
    public interface IMappingPreparerProvider
    {
        IEnumerable<IMappingPreparer> Preparers { get; }
    }
}
