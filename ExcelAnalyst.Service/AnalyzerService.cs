using DocumentFormat.OpenXml.Spreadsheet;
using ClosedXML.Excel;
using Microsoft.Extensions.FileProviders;

namespace ExcelAnalyst.Service
{
    public class AnalyzerService : IAnalyzerService
    {


        public double AnalyzeFuelConsumption(string filePath)
        {
            using var workbook = new XLWorkbook(filePath);
            var sheet = workbook.Worksheet(1);

            int lastRow = sheet.LastRowUsed().RowNumber();
            int firstRow = sheet.FirstRowUsed().RowNumber();
            int lastColumn = sheet.LastColumnUsed().ColumnNumber() - 1;
            int totalCars = 0;
            double totalConsumption = 0;
            for (int row = firstRow + 13; row <= lastRow; row++)
            {
                double sumConsumption = 0;
                int activeDays = 0;

                for (int col = 11; col <= lastColumn; col++) // الأعمدة من J إلى AW
                {
                    var cell = sheet.Cell(row, col);
                    if (double.TryParse(cell.GetValue<string>(), out double consumption) && consumption > 0)
                    {
                        sumConsumption += consumption;
                        activeDays++;
                    }
                }

                if (activeDays >= 11)
                {
                    totalCars += 1;
                    totalConsumption += sumConsumption;
                }
            }

            return (totalCars > 0) ? totalConsumption / totalCars : 0;
        }




    }
}
