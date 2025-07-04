using System.Threading.Tasks;
using ExcelAnalyst.Service.Anlyze;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExcelAnalyst.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AnalyzersController : ControllerBase
    {
        private readonly IAnalyzerService _analyzerService;
        public AnalyzersController(IAnalyzerService analyzerService)
        {
            _analyzerService = analyzerService;
        }

        [HttpPost("analyze-fuel-consumption")]
        public async Task<IActionResult> AnalyzeFuelConsumption(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }
            var extension = Path.GetExtension(file.FileName);
            var tempFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + extension);
            using (var stream = new FileStream(tempFilePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            var result = await _analyzerService.AnalyzeFuelConsumptionAsync(tempFilePath);
            System.IO.File.Delete(tempFilePath);

            if (result.IsFailure)
                return BadRequest(new { message = result.Error.Message});

            return Ok(new { anlyzed = result.Value });
        }
        
    }
}
