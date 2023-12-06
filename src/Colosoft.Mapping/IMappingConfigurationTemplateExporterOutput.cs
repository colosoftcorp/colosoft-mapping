using System.Threading;
using System.Threading.Tasks;

namespace Colosoft.Mapping
{
    public interface IMappingConfigurationTemplateExporterOutput
    {
        Task Write(System.IO.Stream outputStream, CancellationToken cancellationToken);
    }
}