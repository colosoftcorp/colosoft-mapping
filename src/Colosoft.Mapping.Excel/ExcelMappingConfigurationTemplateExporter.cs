using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Colosoft.Mapping.Excel
{
    public class ExcelMappingConfigurationTemplateExporter : IMappingConfigurationTemplateExporter
    {
        private const int DescriptionIndex = 0;
        private const int HeaderIndex = 1;
        private const int TopOffset = 2;
        private const int Lines = 40;

        public string Name => "Excel";

        public string Extension => ".xls";

        public Task<IMappingConfigurationTemplateExporterOutput> Export(IMappingConfiguration configuration, CancellationToken cancellationToken)
        {
            var workbook = new HSSFWorkbook();

            var fontHeader = workbook.CreateFont();
            fontHeader.FontName = "quattrocento sans";
            fontHeader.IsBold = true;

            var fontDescription = workbook.CreateFont();
            fontDescription.FontName = "quattrocento sans";

            var sheet = workbook.CreateSheet(configuration.Name);
            var headerRow = sheet.CreateRow(HeaderIndex);
            var descriptionRow = sheet.CreateRow(DescriptionIndex);

            var cellDescriptionStyle = workbook.CreateCellStyle();
            cellDescriptionStyle.FillPattern = FillPattern.SolidForeground;
            cellDescriptionStyle.Alignment = HorizontalAlignment.Center;
            cellDescriptionStyle.VerticalAlignment = VerticalAlignment.Center;
            cellDescriptionStyle.WrapText = true;
            cellDescriptionStyle.BorderRight = BorderStyle.Medium;
            cellDescriptionStyle.BorderBottom = BorderStyle.Medium;
            cellDescriptionStyle.BorderLeft = BorderStyle.Medium;
            cellDescriptionStyle.SetFont(fontDescription);
            cellDescriptionStyle.FillForegroundColor = IndexedColors.LightCornflowerBlue.Index;

            var colIndex = 0;
            foreach (var field in configuration.Schema.Fields)
            {
                if (field.VisibilityGetter == null || field.VisibilityGetter())
                {
                    var cellHeaderStyle = workbook.CreateCellStyle();
                    cellHeaderStyle.FillPattern = FillPattern.SolidForeground;
                    cellHeaderStyle.Alignment = HorizontalAlignment.Center;
                    cellHeaderStyle.FillForegroundColor = field.IsOptional ? IndexedColors.White.Index : IndexedColors.LightCornflowerBlue.Index;
                    cellHeaderStyle.BorderLeft = BorderStyle.Medium;
                    cellHeaderStyle.BorderRight = BorderStyle.Medium;
                    cellHeaderStyle.BorderBottom = BorderStyle.Thin;

                    var optional = field.IsOptional ? "optional" : "required";
                    var cellDescription = descriptionRow.CreateCell(colIndex);
                    cellDescription.SetCellValue($"({optional}) \n {field.Description}");
                    cellDescription.CellStyle = cellDescriptionStyle;

                    var cellHeader = headerRow.CreateCell(colIndex);
                    cellHeader.SetCellValue(field.Name);
                    cellHeader.CellStyle = cellHeaderStyle;
                    sheet.AutoSizeColumn(colIndex);
                    colIndex++;
                }
            }

            this.CreateInfoRows(workbook, sheet, configuration);

            return Task.FromResult<IMappingConfigurationTemplateExporterOutput>(new ExcelMappingConfigurationTemplateExportOutput(workbook));
        }

        private void CreateInfoRows(HSSFWorkbook workbook, ISheet sheet, IMappingConfiguration configuration)
        {
            var fieldIndex = 0;
            var fieldsConfiguration = configuration.Schema.Fields
                .Where(field => field.VisibilityGetter == null || field.VisibilityGetter())
                .Select(field =>
                {
                    var cellStyle = workbook.CreateCellStyle();
                    cellStyle.FillPattern = FillPattern.SolidForeground;
                    cellStyle.FillForegroundColor = field.IsOptional ? IndexedColors.White.Index : IndexedColors.LightCornflowerBlue.Index;
                    cellStyle.BorderLeft = BorderStyle.Medium;
                    cellStyle.BorderRight = BorderStyle.Medium;

                    return new
                    {
                        Index = fieldIndex++,
                        Field = field,
                        CellStyle = cellStyle,
                    };
                })
                .ToList();

            foreach (var fieldConfiguration in fieldsConfiguration)
            {
                var type = fieldConfiguration.Field.Type;

                if (Nullable.GetUnderlyingType(type) != null)
                {
                    type = type.GenericTypeArguments.FirstOrDefault();
                }

                var options = new List<string>();

                if (fieldConfiguration.Field.OptionsGetter != null)
                {
                    options = fieldConfiguration.Field.OptionsGetter().ToList();
                }
                else if (type.IsEnum)
                {
                    options = Enum.GetValues(type)
                            .Cast<object>()
                            .Select(f => Enum.GetName(type, f))
                            .ToList();
                }

                if (options.Count > 0)
                {
                    var hidenWorkbook = sheet.Workbook;
                    var dropDownName = "List_" + fieldConfiguration.Field.Name.Replace(" ", string.Empty);
                    var hiddenSheet = hidenWorkbook.CreateSheet(dropDownName);

                    for (int i = 0, length = options.Count; i < length; i++)
                    {
                        var row = hiddenSheet.CreateRow(i);
                        var cell = row.CreateCell(0);
                        cell.SetCellValue(options[i]);
                    }

                    var namedCell = hidenWorkbook.CreateName();
                    namedCell.NameName = dropDownName;
                    namedCell.RefersToFormula = dropDownName + "!$A$1:$A$" + options.Count;
                    var constraint = DVConstraint.CreateFormulaListConstraint(dropDownName);
                    var addressList = new NPOI.SS.Util.CellRangeAddressList(TopOffset, Lines, fieldConfiguration.Index, fieldConfiguration.Index);
                    var validation = new HSSFDataValidation(addressList, constraint);
                    var hiddenSheetIndex = hidenWorkbook.GetSheetIndex(hiddenSheet);
                    hidenWorkbook.SetSheetHidden(hiddenSheetIndex, SheetState.Hidden);

                    sheet.AddValidationData(validation);
                }
            }

            for (int i = TopOffset; i <= Lines; i++)
            {
                var infoRow = sheet.CreateRow(i);
                foreach (var fieldConfiguration in fieldsConfiguration)
                {
                    var cellInfo = infoRow.CreateCell(fieldConfiguration.Index);
                    cellInfo.CellStyle = fieldConfiguration.CellStyle;
                    this.SetValue(cellInfo, fieldConfiguration.Field);
                }
            }
        }

        private void SetValue(
            ICell cell,
            IMappingDataSourceSchemaField field)
        {
            var type = field.Type;
            if (Nullable.GetUnderlyingType(type) != null)
            {
                type = type.GenericTypeArguments.FirstOrDefault();
            }

            if (type == typeof(int) || type == typeof(float) || type == typeof(decimal) || type == typeof(double))
            {
                cell.SetCellValue(0);
            }
            else if (type == typeof(bool))
            {
                cell.SetCellValue(false);
            }
            else
            {
                cell.SetCellValue(field.DefaultValue ?? string.Empty);
            }
        }
    }
}
