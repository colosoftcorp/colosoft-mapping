using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Colosoft.Mapping.Excel
{
    internal class ExcelMappingDataSource : IMappingDataSource
    {
        private readonly ISheet sheet;

        public ExcelMappingDataSource(ISheet sheet)
        {
            this.sheet = sheet;
            this.Name = sheet.SheetName;
            var fields = this.GetFields().ToList();
            this.Schema = new ExcelMappingDataSourceSchema(fields);
        }

        public string Name { get; }

        public IMappingDataSourceSchema Schema { get; }

        public Task<IEnumerable<IMappingDataSourceRecord>> GetRecords(CancellationToken cancellationToken)
        {
            var records = new List<IMappingDataSourceRecord>();
            for (int i = 2; i <= this.sheet.LastRowNum; i++)
            {
                var row = this.sheet.GetRow(i);
                if (!this.IsEmpty(row))
                {
                    records.Add(new ExcelMappingDataSourceRecord(row, this.Schema));
                }
            }

            return Task.FromResult<IEnumerable<IMappingDataSourceRecord>>(records);
        }

        private IEnumerable<IMappingDataSourceSchemaField> GetFields()
        {
            var firstRow = this.sheet.GetRow(this.sheet.FirstRowNum);
            var secondRow = this.sheet.GetRow(this.sheet.FirstRowNum + 1);

            for (int i = firstRow.FirstCellNum; i < firstRow.LastCellNum; i++)
            {
                var description = firstRow.GetCell(i).StringCellValue;
                var name = secondRow.GetCell(i).StringCellValue;

                if (string.IsNullOrEmpty(name))
                {
                    throw new InvalidOperationException($"Index {this.sheet.FirstRowNum}:{i} cannot be null");
                }

                yield return new ExcelMappingDataSourceSchemaField
                {
                    Name = name,
                    Description = description,
                    IsOptional = description.IndexOf("optional", StringComparison.InvariantCultureIgnoreCase) >= 0,
                };
            }
        }

        private bool IsEmpty(IRow row)
        {
            var index = 0;
            var limit = this.Schema.Fields.Count(f => !f.IsOptional);
            foreach (var item in this.Schema.Fields)
            {
                if (!item.IsOptional)
                {
                    var cell = row.GetCell(index);

                    if (cell == null ||
                        (cell.CellType == CellType.Numeric && cell.NumericCellValue == 0) ||
                        (cell.CellType == CellType.String && string.IsNullOrWhiteSpace(cell.StringCellValue)) ||
                        cell.CellType == CellType.Blank)
                    {
                        limit--;
                    }
                }

                index++;
            }

            return limit <= 0;
        }
    }
}
