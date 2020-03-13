using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Splat;
using ThaiDust.Core.Model.Persistent;
using Syncfusion.XlsIO;

namespace ThaiDust.Core.Helper
{
    public class ExcelGenerator
    {
        private readonly IFilePicker _filePicker;

        public ExcelGenerator(IFilePicker filePicker = null)
        {
            _filePicker = filePicker ?? Locator.Current.GetService<IFilePicker>();
        }

        public async Task CreateExcel(string stationName, IEnumerable<Record> data)
        {
            using var fileStream = await _filePicker?.CreateFile(stationName);

            using (var excelEngine = new ExcelEngine())
            {
                excelEngine.Excel.DefaultVersion = ExcelVersion.Excel2016;
                var workBook = excelEngine.Excel.Workbooks.Create(0);
                // Group Data
                foreach (var groupedByType in data.GroupBy(p => p.Type).ToArray())
                {
                    IWorksheet workSheet =  workBook.Worksheets.Create(Enum.GetName(typeof(RecordType), groupedByType.Key));
                    // Write Header
                    workSheet.Range["A1"].Text = "Date";
                    workSheet.Range["B1"].Text = "Value";
                    // Write Data
                    AddData2(workSheet, groupedByType);
                }

                await Task.Run(() => workBook.SaveAs(fileStream));
            }
            /*
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
            spreadsheetDocument.Close();*/
        }
        /*
        private void AddHeader(SheetData sheetData)
        {
            var header = new Row { RowIndex = 1 };
            header.AppendChild(new Cell { CellValue = new CellValue("PollutionType"), CellReference = "A1" });
            header.AppendChild(new Cell { CellValue = new CellValue("Date"), CellReference = "B1" });
            header.AppendChild(new Cell { CellValue = new CellValue("Value"), CellReference = "C1" });
            sheetData.AppendChild(header);
        }
        */
        private void AddData2(IWorksheet workSheet, IEnumerable<Record> data)
        {
            for (int i = 0; i < data.Count(); i++)
            {
                var target = data.ElementAt(i);
                workSheet.Range[$"A{2 + i}"].ColumnWidth = 20;
                workSheet.Range[$"A{2 + i}"].DateTime = target.DateTime;
                workSheet.Range[$"A{2 + i}"].NumberFormat = "d/mmm/yyyy h:mm";
                workSheet.Range[$"B{2 + i}"].Text = target.Value.ToString();
            }
        }

        /*
        private void AddData(SheetData sheetData, IEnumerable<Record> data)
        {
            foreach (Record stationValue in data)
            {
                var row = new Row();
                row.AppendChild(new Cell
                { CellValue = new CellValue(Enum.GetName(typeof(RecordType), stationValue.Type)) });
                row.AppendChild(new Cell { CellValue = new CellValue(stationValue.DateTime.ToOADate().ToString(CultureInfo.InvariantCulture)), DataType = new EnumValue<CellValues>(CellValues.Date) });
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
        */
    }
}
