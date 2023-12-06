namespace Colosoft.Mapping
{
    public class MappingDataSourceRecordApplierFactory : IMappingDataSourceRecordApplierFactory
    {
        public IMappingDataSourceRecordApplier<TTarget> Create<TTarget>(IMappingConfiguration configuration)
        {
            return new MappingDataSourceRecordApplier<TTarget>(configuration);
        }
    }
}
