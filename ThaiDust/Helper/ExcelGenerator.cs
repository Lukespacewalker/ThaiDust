using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using ThaiDust.Models;
using X16 = DocumentFormat.OpenXml.Office2016.Excel;

namespace ThaiDust.Helper
{
    public class ExcelGenerator
    {
        public async Task CreateExcel(string stationName, IEnumerable<StationValue> data)
        {
            var file = await FileSystemHelper.CreateFile(stationName);
            if (file == null) return;
            using var fileStream = await file.OpenStreamForWriteAsync();
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

        private void AddData(SheetData sheetData, IEnumerable<StationValue> data)
        {
            foreach (StationValue stationValue in data)
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
