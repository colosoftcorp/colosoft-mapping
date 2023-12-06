using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Colosoft.Mapping.Excel
{
    public abstract class ExcelMappingDataSourceImporter : IFileMappingDataSourceImporter
    {
        protected abstract IEnumerable<string> Extensions { get; }

        public Task<bool> IsCompatible(string fileExtension, Func<CancellationToken, Task<System.IO.Stream>> contentGetter, CancellationToken cancellationToken)
        {
            return Task.FromResult(this.Extensions.Any(f => f.Equals(fileExtension, StringComparison.InvariantCultureIgnoreCase)));
        }

        internal IEnumerable<IMappingDataSource> GetDataSources(NPOI.SS.UserModel.IWorkbook workbook)
        {
            for (int i = 0; i < workbook.NumberOfSheets; i++)
            {
                var sheet = workbook.GetSheetAt(i);
                yield return new ExcelMappingDataSource(sheet);
            }
        }

        protected abstract Task<NPOI.SS.UserModel.IWorkbook> GetWorkbook(System.IO.Stream inputStream, CancellationToken cancellationToken);

        public async Task<IEnumerable<IMappingDataSource>> Import(System.IO.Stream inputStream, CancellationToken cancellationToken)
        {
            var workbook = await this.GetWorkbook(inputStream, cancellationToken);
            return this.GetDataSources(workbook);
        }
    }
}
