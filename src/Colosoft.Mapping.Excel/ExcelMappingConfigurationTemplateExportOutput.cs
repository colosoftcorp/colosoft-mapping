using NPOI.HSSF.UserModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Colosoft.Mapping.Excel
{
    internal class ExcelMappingConfigurationTemplateExportOutput : IMappingConfigurationTemplateExporterOutput
    {
        private readonly HSSFWorkbook excel;

        public ExcelMappingConfigurationTemplateExportOutput(HSSFWorkbook excel)
        {
            this.excel = excel;
        }

        public async Task Write(Stream outputStream, CancellationToken cancellationToken)
        {
            this.excel.Write(outputStream);
            await outputStream.FlushAsync(cancellationToken);
        }
    }
}