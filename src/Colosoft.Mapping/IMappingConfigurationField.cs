using System.Threading.Tasks;

namespace Colosoft.Mapping
{
    public interface IMappingConfigurationField
    {
        string Name { get; }

        Task SetValue(object instance, object value, IMappingContext context);
    }
}
