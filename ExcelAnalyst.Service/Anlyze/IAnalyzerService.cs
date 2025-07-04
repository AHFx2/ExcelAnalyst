using ExcelAnalyst.Domain.Global;
using ExcelAnalyst.Service.Anlyze.DTOs;
using Microsoft.Extensions.FileProviders;

namespace ExcelAnalyst.Service.Anlyze
{
    public interface IAnalyzerService
    {
        Task<Result<AnlayzedDTO>> AnalyzeFuelConsumptionAsync(string filePath);
    }
}