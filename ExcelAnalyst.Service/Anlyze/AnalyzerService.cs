using DocumentFormat.OpenXml.Spreadsheet;
using ClosedXML.Excel;
using Microsoft.Extensions.FileProviders;
using ExcelAnalyst.Service.Anlyze.DTOs;
using ExcelAnalyst.Domain.Global;
using ExcelAnalyst.Domain.Logger;
using ExcelAnalyst.Domain.Common.IRepositores;
using System.Threading.Tasks;
using ExcelAnalyst.Domain.Entities;

namespace ExcelAnalyst.Service.Anlyze
{
    public class AnalyzerService : IAnalyzerService
    {
        private readonly ExcelAnalyst.Domain.Logger.ILogger<AnalyzerService> _logger;
        private readonly IAnalystRepository _analystRepository;
        public AnalyzerService(ExcelAnalyst.Domain.Logger.ILogger<AnalyzerService> logger, IAnalystRepository analystRepository)
        {
            _logger = logger;
            _analystRepository = analystRepository;
        }


        public async Task<Result<AnlayzedDTO>> AnalyzeFuelConsumptionAsync(string filePath)
        {

            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                _logger.LogError("File path is null or file does not exist: {FilePath}", null, filePath);
                return Result.Failure<AnlayzedDTO>(Error.General.NullValue);
            }

            try
            {
                using var workbook = new XLWorkbook(filePath);
                var sheet = workbook.Worksheet(1);

                if (workbook.Worksheets == null)
                {
                    _logger.LogError("No worksheets found in the workbook: {FilePath}", null, filePath);
                    return Result.Failure<AnlayzedDTO>(Error.General.NullValue);
                }

                var date = sheet.Cell("D10").GetValue<DateTime>();
                var department = sheet.Cell("B14").GetValue<string>();

                if (await _analystRepository.IsExsistAsync(department, date))
                {
                    return Result.Failure<AnlayzedDTO>(new Error("Error.AnalyzerService", "This file has been anlyzed"));
                }
                var consumers = new List<FuelConsumerDTO>();
                int totalCars = 0;
                double totalConsumption = 0;

                for (int row = sheet.FirstRowUsed().RowNumber() + 12; row <= (sheet.LastRowUsed().RowNumber() - 2); row++)
                {
                    totalCars++;
                    double sumConsumption = 0;
                    int activeDays = 0;

                    for (int col = 11; col <= sheet.LastColumnUsed().ColumnNumber() - 1; col++)
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
                        totalConsumption += sumConsumption;
                        consumers.Add(GetFuelConsumerDTO(sheet.Cell(row, 8).GetValue<string>(), Math.Round(sumConsumption, 2), activeDays));
                    }
                }

                if (!(await _analystRepository.AddAsync(new Analyst { Date = date, Department = department })))
                {
                    throw new Exception("Failed to save analysis data to the repository.");
                }

                AnlayzedDTO anlyzedData = new AnlayzedDTO
                {
                    Consumption = (consumers.Count > 0) ? Math.Round(totalConsumption / consumers.Count, 2) : 0,
                    ActiveCars = consumers.Count,
                    InactiveCars = totalCars - consumers.Count,
                    TotalCars = totalCars,
                    HighestConsumer = consumers.Count > 0 ? consumers.Max(c => c.Consumption) : 0,
                    LowestConsumer = consumers.Count > 0 ? consumers.Min(c => c.Consumption) : 0,
                    FuelConsumers = consumers.OrderBy(c => c.Consumption).ToList(),

                };

                return Result.Success(anlyzedData);
            }

            catch (Exception ex)
            {
                _logger.LogCritical("An error occurred while analyzing the file: {FilePath}, Error: {ErrorMessage}", null, filePath, ex.Message);
                return Result.Failure<AnlayzedDTO>(new Error("Error.AnalyzerService", $"An error occurred while analyzing the file: {ex.Message}"));
            }

        }


        private FuelConsumerDTO GetFuelConsumerDTO(string plate, double consumption, int activeDays)
        {
            return new FuelConsumerDTO
            {
                Plate = plate,
                Consumption = consumption,
                ActiveDays = activeDays
            };
        }




    }
}
