namespace Colosoft.Mapping
{
    public interface IMappingDataSourceRecordApplierFactory
    {
        IMappingDataSourceRecordApplier<TTarget> Create<TTarget>(IMappingConfiguration configuration);
    }
}
