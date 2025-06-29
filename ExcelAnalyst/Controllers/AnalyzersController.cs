using ExcelAnalyst.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExcelAnalyst.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnalyzersController : ControllerBase
    {
        private readonly IAnalyzerService _analyzerService;
        public AnalyzersController(IAnalyzerService analyzerService)
        {
            _analyzerService = analyzerService;
        }

        [HttpPost("analyze-fuel-consumption")]
        public IActionResult AnalyzeFuelConsumption(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }
            var tempFilePath = Path.GetTempFileName();
            using (var stream = new FileStream(tempFilePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }
            try
            {
                var result = _analyzerService.AnalyzeFuelConsumption(tempFilePath);
                return Ok(new { AverageConsumption = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
            finally
            {
                System.IO.File.Delete(tempFilePath); // Clean up the temporary file
            }
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadExcel(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("❌ لم يتم رفع ملف.");

            string filePath = Path.Combine(Path.GetTempPath(), file.FileName);

            using (var stream = System.IO.File.Create(filePath))
            {
                await file.CopyToAsync(stream);
            }

            // الحين تقدر تمرر filePath للدالة اللي تحلل الملف
            var result = _analyzerService. AnalyzeFuelConsumption(filePath);
            return Ok(result);
        }
    }
}
