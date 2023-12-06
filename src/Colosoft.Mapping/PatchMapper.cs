using Colosoft.Input;
using System.Collections.Generic;
using System.Linq;

namespace Colosoft.Mapping
{
    public abstract class PatchMapper<TSource, TTarget> : Mapper<TSource, TTarget>
        where TSource : IPatchInput
    {
        protected override IEnumerable<IPropertyMap<TSource, TTarget>> GetChangedProperties(TSource source)
        {
            if (source != null)
            {
                var changes = source.GetChanges().ToList();
                return this.Properties.Where(f => changes.Contains(f.PropertyName));
            }

            return new IPropertyMap<TSource, TTarget>[0];
        }
    }
}
