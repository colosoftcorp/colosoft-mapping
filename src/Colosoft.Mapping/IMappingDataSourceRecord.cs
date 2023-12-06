namespace Colosoft.Mapping
{
    public interface IMappingDataSourceRecord
    {
        IMappingDataSourceSchema Schema { get; }

        object GetValue(string fieldName);
    }
}