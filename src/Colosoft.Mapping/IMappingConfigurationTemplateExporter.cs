using System.Threading;
using System.Threading.Tasks;

namespace Colosoft.Mapping
{
    public interface IMappingConfigurationTemplateExporter
    {
        string Name { get; }

        string Extension { get; }

        Task<IMappingConfigurationTemplateExporterOutput> Export(IMappingConfiguration configuration, CancellationToken cancellationToken);
    }
}
