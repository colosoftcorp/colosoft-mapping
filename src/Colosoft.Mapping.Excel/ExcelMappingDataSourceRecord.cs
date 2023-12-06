using NPOI.SS.UserModel;
using System.Collections.Generic;

namespace Colosoft.Mapping.Excel
{
    internal class ExcelMappingDataSourceRecord : IMappingDataSourceRecord
    {
        private readonly IRow row;

        public ExcelMappingDataSourceRecord(IRow row, IMappingDataSourceSchema schema)
        {
            this.row = row;
            this.Schema = schema;
        }

        public IMappingDataSourceSchema Schema { get; }

        public object GetValue(string fieldName)
        {
            var index = 0;
            foreach (var field in this.Schema.Fields)
            {
                if (System.StringComparer.InvariantCultureIgnoreCase.Equals(field.Name, fieldName))
                {
                    var current = this.row.GetCell(index);
                    if (current == null)
                    {
                        return null;
                    }
                    else if (current.CellType == CellType.Numeric)
                    {
                        return current.NumericCellValue;
                    }
                    else if (current.CellType == CellType.Boolean)
                    {
                        return current.BooleanCellValue;
                    }

                    return current.StringCellValue;
                }

                index++;
            }

            throw new KeyNotFoundException(fieldName);
        }
    }
}
