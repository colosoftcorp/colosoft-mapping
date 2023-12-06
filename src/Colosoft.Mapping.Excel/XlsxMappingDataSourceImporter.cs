using NPOI.SS.UserModel;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Colosoft.Mapping.Excel
{
    public class XlsxMappingDataSourceImporter : ExcelMappingDataSourceImporter
    {
        protected override IEnumerable<string> Extensions { get; } = new[] { ".xlsx" };

        protected override Task<IWorkbook> GetWorkbook(Stream inputStream, CancellationToken cancellationToken) =>
            Task.FromResult<IWorkbook>(new NPOI.XSSF.UserModel.XSSFWorkbook(inputStream));
    }
}
