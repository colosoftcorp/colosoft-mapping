namespace Colosoft.Mapping
{
    public interface IPropertyMapContextSupport<in TSource, in TTarget> : IPropertyMap<TSource, TTarget>
    {
        void Apply(TSource source, TTarget target, IMappingContext context);
    }
}
