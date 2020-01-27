using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Splat;
using ThaiDust.Core.Model.Persistent;

namespace ThaiDust.Core.Helper
{
    public class ExcelGenerator
    {
        private IFilePicker _filePicker;

        public ExcelGenerator(IFilePicker filePicker = null)
        {
            _filePicker = filePicker ?? Locator.Current.GetService<IFilePicker>();
        }


        public async Task CreateExcel(string stationName, IEnumerable<Record> data)
        {
            using var fileStream = await _filePicker?.CreateFile(stationName);
            // Create a spreadsheet document by supplying the filepath.
            // By default, AutoSave = true, Editable = true, and Type = xlsx.
            SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Create(fileStream, SpreadsheetDocumentType.Workbook);

            // Add a WorkbookPart to the document.
            WorkbookPart workbookpart = spreadsheetDocument.AddWorkbookPart();
            workbookpart.Workbook = new Workbook(); 

            // Add a WorksheetPart to the WorkbookPart.
            WorksheetPart worksheetPart = workbookpart.AddNewPart<WorksheetPart>();
            worksheetPart.Worksheet = new Worksheet(new SheetData());

            // Add Sheets to the Workbook.
            Sheets sheets = spreadsheetDocument.WorkbookPart.Workbook.AppendChild<Sheets>(new Sheets());

            // Append a new worksheet and associate it with the workbook.
            Sheet sheet = new Sheet() { Id = spreadsheetDocument.WorkbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = stationName };
            sheets.Append(sheet);

            // Get the sheetData cell table.
            SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

            AddHeader(sheetData);
            AddData(sheetData, data);

            workbookpart.Workbook.Save();

            // Close the document.
            spreadsheetDocument.Close();
        }

        private void AddHeader(SheetData sheetData)
        {
            var header = new Row { RowIndex = 1 };
            header.AppendChild(new Cell { CellValue = new CellValue("Date"), CellReference = "A1" });
            header.AppendChild(new Cell { CellValue = new CellValue("Value"), CellReference = "B1" });
            sheetData.AppendChild(header);
        }

        private void AddData(SheetData sheetData, IEnumerable<Record> data)
        {
            foreach (Record stationValue in data)
            {
                var row = new Row();
                row.AppendChild(new Cell { CellValue = new CellValue(stationValue.DateTime.ToOADate().ToString(CultureInfo.InvariantCulture)), DataType = new EnumValue<CellValues>(CellValues.Number) });
                row.AppendChild(stationValue.Value == null
                    ? new Cell { CellValue = new CellValue(), DataType = new EnumValue<CellValues>(CellValues.Number) }
                    : new Cell
                    {
                        CellValue = new CellValue(stationValue.Value.ToString()),
                        DataType = new EnumValue<CellValues>(CellValues.Number)
                    });
                sheetData.AppendChild(row);
            }
        }

    }
}
