using Microsoft.Extensions.FileProviders;

namespace ExcelAnalyst.Service
{
    public interface IAnalyzerService
    {
        double AnalyzeFuelConsumption(string filePath);
    }
}